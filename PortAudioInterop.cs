using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#region Typedefs
using PaError			= Commons.Media.PortAudio.PaErrorCode;		// typedef int 	PaError
using PaDeviceIndex		= System.Int32;		// typedef int 	PaDeviceIndex
using PaHostApiIndex		= System.Int32;		// typedef int 	PaHostApiIndex
using PaTime			= System.Double;	// typedef double 	PaTime
using PaSampleFormat		= System.UInt64;	// typedef unsigned long 	PaSampleFormat
using PaStream			= System.Void;		// typedef void 	PaStream
using PaStreamFlags		= System.UInt64;	// typedef unsigned long 	PaStreamFlags
#endregion

namespace Commons.Media.PortAudio
{
	#region Typedefs -> delegates
	public delegate int 	PaStreamCallback (/*const*/ IntPtr/*void **/input, IntPtr/**void **/output, /*unsigned*/ ulong frameCount, /*const*/ IntPtr/*PaStreamCallbackTimeInfo **/timeInfo, PaStreamCallbackFlags statusFlags, IntPtr/*void **/userData);
	public delegate void 	PaStreamFinishedCallback (IntPtr/*void **/userData);
	#endregion
	
	public static class PortAudioInterop
	{
		static PortAudioInterop ()
		{
			Pa_Initialize ();
			AppDomain.CurrentDomain.DomainUnload += (o, args) => Pa_Terminate ();
		}
		
		#region Functions
		[DllImport ("portaudio")] public static extern
		int 	Pa_GetVersion (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		/*const*/ string/*char * */ 	Pa_GetVersionText (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		/*const*/ string/*char * */ 	Pa_GetErrorText (PaError errorCode)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_Initialize (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_Terminate (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		PaHostApiIndex 	Pa_GetHostApiCount (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		PaHostApiIndex 	Pa_GetDefaultHostApi (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		/*const*/ IntPtr/*PaHostApiInfo * */ 	Pa_GetHostApiInfo (PaHostApiIndex hostApi)
			;
		[DllImport ("portaudio")] public static extern
		PaHostApiIndex 	Pa_HostApiTypeIdToHostApiIndex (PaHostApiTypeId type)
			;
		[DllImport ("portaudio")] public static extern
		PaDeviceIndex 	Pa_HostApiDeviceIndexToDeviceIndex (PaHostApiIndex hostApi, int hostApiDeviceIndex)
			;
		[DllImport ("portaudio")] public static extern
		/*const*/ IntPtr/*PaHostErrorInfo * */ 	Pa_GetLastHostErrorInfo (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		PaDeviceIndex 	Pa_GetDeviceCount (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		PaDeviceIndex 	Pa_GetDefaultInputDevice (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		PaDeviceIndex 	Pa_GetDefaultOutputDevice (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		/*const*/ IntPtr/*PaDeviceInfo * */ 	Pa_GetDeviceInfo (PaDeviceIndex device)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_IsFormatSupported (/*const*/ IntPtr/*PaStreamParameters * */inputParameters, /*const*/ 
		IntPtr/*PaStreamParameters * */outputParameters, double sampleRate)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_OpenStream (out IntPtr/*PaStream ** */stream, /*const*/ IntPtr/*PaStreamParameters * */inputParameters, /*const*/ IntPtr/*PaStreamParameters * */outputParameters, double sampleRate, /*unsigned*/ulong framesPerBuffer, PaStreamFlags streamFlags, PaStreamCallback streamCallback, IntPtr/*void **/userData)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_OpenDefaultStream (out IntPtr/*PaStream ** */stream, int numInputChannels, int numOutputChannels, PaSampleFormat sampleFormat, double sampleRate, /*unsigned*/ulong framesPerBuffer, PaStreamCallback streamCallback, IntPtr/*void **/userData)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_CloseStream (IntPtr/*PaStream * */stream)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_SetStreamFinishedCallback (IntPtr/*PaStream * */stream, PaStreamFinishedCallback streamFinishedCallback)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_StartStream (IntPtr/*PaStream * */stream)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_StopStream (IntPtr/*PaStream * */stream)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_AbortStream (IntPtr/*PaStream * */stream)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_IsStreamStopped (IntPtr/*PaStream * */stream)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_IsStreamActive (IntPtr/*PaStream * */stream)
			;
		[DllImport ("portaudio")] public static extern
		/*const*/ IntPtr/*PaStreamInfo * */	Pa_GetStreamInfo (IntPtr/*PaStream * */stream)
			;
		[DllImport ("portaudio")] public static extern
		PaTime 	Pa_GetStreamTime (IntPtr/*PaStream * */stream)
			;
		[DllImport ("portaudio")] public static extern
		double 	Pa_GetStreamCpuLoad (IntPtr/*PaStream * */stream)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_ReadStream (IntPtr/*PaStream * */stream, IntPtr/*void **/buffer, /*unsigned*/ulong frames)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_WriteStream (IntPtr/*PaStream * */stream, /*const*/ IntPtr/*void **/buffer, /*unsigned*/ulong frames)
			;
		[DllImport ("portaudio")] public static extern
		/*signed*/ long 	Pa_GetStreamReadAvailable (IntPtr/*PaStream * */stream)
			;
		[DllImport ("portaudio")] public static extern
		/*signed*/ long 	Pa_GetStreamWriteAvailable (IntPtr/*PaStream * */stream)
			;

		// Return value is originally defined as PaError but this should rather make sense.
		[DllImport ("portaudio")] public static extern
		int 	Pa_GetSampleSize (PaSampleFormat format)
			;
		[DllImport ("portaudio")] public static extern
		void 	Pa_Sleep (long msec)
			;
		#endregion
	}

#if !USE_CXXI

	public static class Factory
	{
		public static CppInstancePtr ToNative<T> (T value)
		{
			IntPtr ret = Marshal.AllocHGlobal (Marshal.SizeOf (value));
			Marshal.StructureToPtr (value, ret, false);
			return CppInstancePtr.Create<T> (ret);
		}
		
		public static T Create<T> (CppInstancePtr handle)
		{
			return (T) Marshal.PtrToStructure (handle.Native, typeof (T));
		}
		
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct PaHostApiInfo
	{
		public int 	structVersion;
		public PaHostApiTypeId 	type;
		[MarshalAs (UnmanagedType.LPStr)]
		public string 	name;
		public int 	deviceCount;
		public PaDeviceIndex 	defaultInputDevice;
		public PaDeviceIndex 	defaultOutputDevice;
	}
	
	[StructLayout (LayoutKind.Sequential)]
	public struct PaHostErrorInfo
	{
		public PaHostApiTypeId 	hostApiType;
		public long 	errorCode;
		[MarshalAs (UnmanagedType.LPStr)]
		public string 	errorText;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct PaDeviceInfo
	{
		public int 	structVersion;
		[MarshalAs (UnmanagedType.LPStr)]
		public string 	name;
		public PaHostApiIndex 	hostApi;
		public int 	maxInputChannels;
		public int 	maxOutputChannels;
		public PaTime 	defaultLowInputLatency;
		public PaTime 	defaultLowOutputLatency;
		public PaTime 	defaultHighInputLatency;
		public PaTime 	defaultHighOutputLatency;
		public double 	defaultSampleRate;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct PaStreamParameters 
	{
		public PaDeviceIndex 	device;
		public int 	channelCount;
		public PaSampleFormat 	sampleFormat;
		public PaTime 	suggestedLatency;
		public IntPtr 	hostApiSpecificStreamInfo;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct PaStreamCallbackTimeInfo 
	{
		public PaTime 	inputBufferAdcTime;
		public PaTime 	currentTime;
		public PaTime 	outputBufferDacTime;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct PaStreamInfo 
	{
		public int 	structVersion;
		public PaTime 	inputLatency;
		public PaTime 	outputLatency;
		public double 	sampleRate;
	}
	
	public struct CppInstancePtr : IDisposable
	{
		IntPtr ptr;
		bool delete;
		Type type;
		
		public CppInstancePtr (IntPtr ptr)
		{
			this.ptr = ptr;
			delete = false;
			type = null;
		}
		
		public static CppInstancePtr Create<T> (IntPtr ptr)
		{
			return new CppInstancePtr (ptr, typeof (T));
		}
		
		CppInstancePtr (IntPtr ptr, Type type)
		{
			this.ptr = ptr;
			this.delete = true;
			this.type = type;
		}

		public void Dispose ()
		{
			if (delete)
				Marshal.DestroyStructure (ptr, type);
		}
		
		public IntPtr Native {
			get { return ptr; }
		}
	}
#endif

}



