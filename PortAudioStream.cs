using System;
using Mono.Cxxi;

namespace Commons.Media.PortAudio
{
	public class PortAudioStream : IDisposable
	{
		IntPtr handle;
		
		public PortAudioStream (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentNullException ("handle");
			this.handle = handle;
			should_dispose_handle = false;
		}
		
		public PortAudioStream (PaStreamParameters inputParameters, PaStreamParameters outputParameters, double sampleRate, ulong framesPerBuffer, ulong streamFlags, StreamCallback streamCallback, IntPtr userData)
		{
			HandleError (PortAudioInterop.Pa_OpenStream (out handle, inputParameters.Native.Native, outputParameters.Native.Native, sampleRate, framesPerBuffer, streamFlags, ToPaStreamCallback (streamCallback), userData));
		}
		
		public PortAudioStream (int numInputChannels, int numOutputChannels, ulong sampleFormat, double sampleRate, ulong framesPerBuffer, StreamCallback streamCallback, IntPtr userData)
		{
			HandleError (PortAudioInterop.Pa_OpenDefaultStream (out handle, numInputChannels, numOutputChannels, sampleFormat, sampleRate, framesPerBuffer, ToPaStreamCallback (streamCallback), userData));
		}
		
		PaStreamCallback ToPaStreamCallback (StreamCallback src)
		{
			return (input, output, frameCount, timeInfo, statusFlags, userData) => {
				var ptr = timeInfo != IntPtr.Zero ? new CppInstancePtr (timeInfo) : default (CppInstancePtr);
				try {
					return src (input, output, frameCount, timeInfo != IntPtr.Zero ? new PaStreamCallbackTimeInfo (ptr) : null, statusFlags, userData);
				} finally {
					ptr.Dispose ();
				}
			};
		}
		
		bool should_dispose_handle = true;
		
		public void Close ()
		{
			if (handle != IntPtr.Zero) {
				if (should_dispose_handle)
					HandleError (PortAudioInterop.Pa_CloseStream (handle));
				handle = IntPtr.Zero;
			}
		}
		
		public void Dispose ()
		{
			Close ();
		}

		public delegate int StreamCallback (IntPtr input, IntPtr output, ulong frameCount, PaStreamCallbackTimeInfo timeInfo, ulong statusFlags, IntPtr userData);
		public delegate void 	StreamFinishedCallback (IntPtr userData);
		
		public void SetStreamFinishedCallback (StreamFinishedCallback streamFinishedCallback)
		{
			HandleError (PortAudioInterop.Pa_SetStreamFinishedCallback (handle, userData => streamFinishedCallback (userData)));
		}
		
		public void StartStream ()
		{
			HandleError (PortAudioInterop.Pa_StartStream (handle));
		}
		
		public void StopStream ()
		{
			HandleError (PortAudioInterop.Pa_StopStream (handle));
		}
		
		public void AbortStream ()
		{
			HandleError (PortAudioInterop.Pa_AbortStream (handle));
		}
		
		public bool IsStopped {
			get {
				var ret = HandleError (PortAudioInterop.Pa_IsStreamStopped (handle));
				return (int) ret != 0;
			}
		}
		
		public bool IsActive {
			get {
				var ret = HandleError (PortAudioInterop.Pa_IsStreamActive (handle));
				return (int) ret != 0;
			}
		}
		
		public PaStreamInfo StreamInfo {
			get {
				var ptr = PortAudioInterop.Pa_GetStreamInfo (handle);
				if (ptr == IntPtr.Zero)
					ThrowLastError ();
				using (var cppptr = new CppInstancePtr (ptr))
					return new PaStreamInfo (cppptr);
			}
		}
		
		public double StreamTime {
			get {
				var ret = PortAudioInterop.Pa_GetStreamTime (handle);
				if (ret == 0)
					ThrowLastError ();
				return ret;
			}
		}
		
		public double CpuLoad {
			get {
				var ret = PortAudioInterop.Pa_GetStreamCpuLoad (handle);
				if (ret == 0)
					ThrowLastError ();
				return ret;
			}
		}
		
		// "The function doesn't return until the entire buffer has been filled - this may involve waiting for the operating system to supply the data." (!)
		public void Read (byte [] buffer, int offset, ulong frames)
		{
			unsafe {
				fixed (byte* buf = buffer)
					HandleError (PortAudioInterop.Pa_ReadStream (handle, (IntPtr) (buf + offset), frames));
			}
		}
		
		// "The function doesn't return until the entire buffer has been filled - this may involve waiting for the operating system to supply the data." (!)
		public void Write (byte [] buffer, int offset, ulong frames)
		{
			unsafe {
				fixed (byte* buf = buffer)
					HandleError (PortAudioInterop.Pa_WriteStream (handle, (IntPtr) (buf + offset), frames));
			}
		}
		
		public long AvailableReadFrames {
			get {
				var ret = PortAudioInterop.Pa_GetStreamReadAvailable (handle);
				if (ret < 0)
					HandleError ((PaErrorCode) ret);
				return ret;
			}
		}
		
		public long AvailableWriteFrames  {
			get {
				var ret = PortAudioInterop.Pa_GetStreamWriteAvailable (handle);
				if (ret < 0)
					HandleError ((PaErrorCode) ret);
				return ret;
			}
		}
		
		static PaErrorCode HandleError (PaErrorCode errorCode)
		{
			return Configuration.HandleError (errorCode);
		}
		
		static void ThrowLastError ()
		{
			Configuration.ThrowLastError ();
		}
	}
}
