using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#region Typedefs
using PaError			= System.Int32;		// typedef int 	PaError
using PaDeviceIndex		= System.Int32;		// typedef int 	PaDeviceIndex
using PaHostApiIndex		= System.Int32;		// typedef int 	PaHostApiIndex
using PaTime			= System.Double;	// typedef double 	PaTime
using PaSampleFormat		= System.UInt64;	// typedef unsigned long 	PaSampleFormat
using PaStream			= System.Void;		// typedef void 	PaStream
using PaStreamFlags		= System.UInt64;	// typedef unsigned long 	PaStreamFlags
using PaStreamCallbackFlags	= System.UInt64;	// typedef unsigned long 	PaStreamCallbackFlags
//typedef int 	PaStreamCallback (const IntPtr/*void **/input, IntPtr/*void **/output, unsigned long frameCount, const PaStreamCallbackTimeInfo *timeInfo, PaStreamCallbackFlags statusFlags, IntPtr/*void **/userData)
//typedef void 	PaStreamFinishedCallback (IntPtr/*void **/userData)
#endregion

namespace Commons.Media.PortAudio
{
	#region Typedefs -> delegates
	public delegate int 	PaStreamCallback (/*const*/ IntPtr/*IntPtr/*void **/input, IntPtr/*IntPtr/*void **/output, /*unsigned*/ ulong frameCount, /*const*/ IntPtr/*PaStreamCallbackTimeInfo **/timeInfo, PaStreamCallbackFlags statusFlags, IntPtr/*IntPtr/*void **/userData);
	public delegate void 	PaStreamFinishedCallback (IntPtr/*IntPtr/*void **/userData);
	#endregion
	
	public static class PortAudioInterop
	{
		#region Functions
		[DllImport ("portaudio")] public static extern
		int 	Pa_GetVersion (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		/*const*/ IntPtr/*char * */ 	Pa_GetVersionText (/*void*/)
			;
		[DllImport ("portaudio")] public static extern
		/*const*/ IntPtr/*char * */ 	Pa_GetErrorText (PaError errorCode)
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
		PaError 	Pa_IsFormatSupported (/*const*/ IntPtr/*IntPtr/*PaStreamParameters * */inputParameters, /*const*/ 
		IntPtr/*IntPtr/*PaStreamParameters * */outputParameters, double sampleRate)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_OpenStream (out IntPtr/*PaStream ** */stream, /*const*/ IntPtr/*PaStreamParameters * */inputParameters, /*const*/ IntPtr/*PaStreamParameters * */outputParameters, double sampleRate, /*unsigned*/ulong framesPerBuffer, PaStreamFlags streamFlags, IntPtr/*PaStreamCallback * */streamCallback, IntPtr/*void **/userData)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_OpenDefaultStream (out IntPtr/*PaStream ** */stream, int numInputChannels, int numOutputChannels, PaSampleFormat sampleFormat, double sampleRate, /*unsigned*/ulong framesPerBuffer, IntPtr/*PaStreamCallback * */streamCallback, IntPtr/*void **/userData)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_CloseStream (IntPtr/*PaStream * */stream)
			;
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_SetStreamFinishedCallback (IntPtr/*PaStream * */stream, IntPtr/*PaStreamFinishedCallback * */streamFinishedCallback)
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
		[DllImport ("portaudio")] public static extern
		PaError 	Pa_GetSampleSize (PaSampleFormat format)
			;
		[DllImport ("portaudio")] public static extern
		void 	Pa_Sleep (long msec)
			;
		#endregion
	}
}



