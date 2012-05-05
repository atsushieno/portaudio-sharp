using System;
using System.Runtime.InteropServices;

namespace Commons.Media.PortAudio
{
	public class PortAudioInputStream : PortAudioStream
	{
		public PortAudioInputStream (PaStreamParameters inputParameters, double sampleRate, ulong framesPerBuffer, PaStreamFlags streamFlags, StreamCallback streamCallback, IntPtr userData)
		{
			using (var input = Factory.ToNative<PaStreamParameters> (inputParameters))
				HandleError (PortAudioInterop.Pa_OpenStream (out handle, input.Native, IntPtr.Zero, sampleRate, framesPerBuffer, streamFlags, ToPaStreamCallback (streamCallback, false), userData));
		}
		
		public PortAudioInputStream (int numInputChannels, ulong sampleFormat, double sampleRate, ulong framesPerBuffer, StreamCallback streamCallback, IntPtr userData)
		{
			HandleError (PortAudioInterop.Pa_OpenDefaultStream (out handle, numInputChannels, 0, sampleFormat, sampleRate, framesPerBuffer, ToPaStreamCallback (streamCallback, false), userData));
		}
	}
	
	public class PortAudioOutputStream : PortAudioStream
	{
		public PortAudioOutputStream (PaStreamParameters outputParameters, double sampleRate, ulong framesPerBuffer, PaStreamFlags streamFlags, StreamCallback streamCallback, IntPtr userData)
		{
			using (var output = Factory.ToNative<PaStreamParameters> (outputParameters))
				HandleError (PortAudioInterop.Pa_OpenStream (out handle, IntPtr.Zero, output.Native, sampleRate, framesPerBuffer, streamFlags, ToPaStreamCallback (streamCallback, true), userData));
		}
		
		public PortAudioOutputStream (int numOutputChannels, ulong sampleFormat, double sampleRate, ulong framesPerBuffer, StreamCallback streamCallback, IntPtr userData)
		{
			HandleError (PortAudioInterop.Pa_OpenDefaultStream (out handle, 0, numOutputChannels, sampleFormat, sampleRate, framesPerBuffer, ToPaStreamCallback (streamCallback, true), userData));
		}
	}

	public abstract class PortAudioStream : IDisposable
	{
		internal IntPtr handle;
		
		protected PortAudioStream ()
		{
		}
		
		protected PortAudioStream (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentNullException ("handle");
			this.handle = handle;
			should_dispose_handle = false;
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

		public delegate PaStreamCallbackResult StreamCallback (byte [] buffer, int offset, int byteCount, PaStreamCallbackTimeInfo timeInfo, PaStreamCallbackFlags statusFlags, IntPtr userData);
		public delegate void StreamFinishedCallback (IntPtr userData);
		
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
					return Factory.Create<PaStreamInfo> (cppptr);
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
		
		internal static PaErrorCode HandleError (PaErrorCode errorCode)
		{
			return Configuration.HandleError (errorCode);
		}
		
		internal static void ThrowLastError ()
		{
			Configuration.ThrowLastError ();
		}
		
		WeakReference buffer;
		
		int FramesToBytes (ulong frames)
		{
			return (int) frames;
		}
		
		internal unsafe PaStreamCallback ToPaStreamCallback (StreamCallback src, bool isOutput)
		{
			return (input, output, frameCount, timeInfo, statusFlags, userData) => {
				var ptr = timeInfo != IntPtr.Zero ? new CppInstancePtr (timeInfo) : default (CppInstancePtr);
				try {
					byte [] buf = buffer != null ? (byte []) buffer.Target : null;
					var byteCount = FramesToBytes (frameCount);
					if (buf == null || buf.Length < byteCount) {
						buf = new byte [byteCount];
						buffer = new WeakReference (buf);
					}
					var ret = src (buf, 0, byteCount, timeInfo != IntPtr.Zero ? Factory.Create<PaStreamCallbackTimeInfo> (ptr) : default (PaStreamCallbackTimeInfo), statusFlags, userData);
					Marshal.Copy (buf, 0, isOutput ? output : input, byteCount);
					return ret;
				} finally {
					ptr.Dispose ();
				}
			};
		}
	}
}
