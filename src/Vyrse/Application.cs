using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading;
using Vyrse.Core;
using Vyrse.Platform.Windows.Win32;
using Vyrse.Tools;

namespace Vyrse;

using static ComGuidHarvester;

public sealed class Application
{
	private Kernel Kernel { get; set; } = null!;
	internal static readonly CancellationTokenSource Cts = new();

	[STAThread]
	internal static int Main(string[] args) => (int)new Application().Run(args);

	private ExitCode Run(string[] args)
	{
		// Generate(new Config
		// {
		// 	Root =
		// 	[
		// 		new Root
		// 		{
		// 			Path = @"C:\Program Files (x86)\Windows Kits\10\Include\10.0.26100.0",
		// 			Namespace = "Windows.Win32.System.Com",
		// 			ClassName = "IID",
		// 			OutputDirectory = @"C:\Dev\C#\Vyrse\src\Vyrse.Interop.Windows\Win32\System\Com",
		// 			Headers =
		// 			[
		// 				new Header { Path = @"um\Unknwn.h", ClassName = "IID", Prefixes = ["IUnknown"] },
		//
		// 				new Header { Path = @"um\d3d11.h", ClassName = "D3D11", Prefixes = ["ID3D11"] },
		// 				new Header { Path = @"um\d3d11_1.h", ClassName = "D3D11", Prefixes = ["ID3D11"] },
		// 				new Header { Path = @"um\d3d11_2.h", ClassName = "D3D11", Prefixes = ["ID3D11"] },
		// 				new Header { Path = @"um\d3d11_3.h", ClassName = "D3D11", Prefixes = ["ID3D11"] },
		// 				new Header { Path = @"um\d3d11_4.h", ClassName = "D3D11", Prefixes = ["ID3D11"] },
		// 				new Header { Path = @"um\d3d11shader.h", ClassName = "D3D11", Prefixes = ["ID3D11"] },
		// 				new Header { Path = @"um\d3d11sdklayers.h", ClassName = "D3D11", Prefixes = ["ID3D11"] },
		// 				new Header { Path = @"um\d3d11on12.h", ClassName = "D3D11", Prefixes = ["ID3D11"] },
		//
		// 				new Header { Path = @"shared\dxgi.h", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgi1_2.h", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgi1_3.h", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgi1_4.h", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgi1_5.h", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgi1_6.h", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgidebug.h", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		//
		// 				new Header { Path = @"shared\dxgi.idl", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgi1_2.idl", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgi1_3.idl", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgi1_4.idl", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgi1_5.idl", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgi1_6.idl", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		// 				new Header { Path = @"shared\dxgidebug.idl", ClassName = "DXGI", Prefixes = ["IDXGI"] },
		//
		// 				new Header { Path = @"um\GameInput.h", ClassName = "GameInput", },
		// 			]
		// 		}
		// 	]
		// });

		Kernel = new Kernel
		{
			Context = new Context()
		};

		var code = Kernel.Run();

		Window.Unregister();
		Cts.Dispose();

		return code;
	}

	[Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsRunning() => !Cts.IsCancellationRequested;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Shutdown() => Cts.Cancel();

	public enum ExitCode
	{
		/// clean shutdown
		Ok = 0,
		/// normal cancellation (e.g., user quit)
		Canceled = 10,
		/// non-fatal error; runtime can shut down gracefully
		Recoverable = 20,
		/// fatal error
		Fatal = 30
	}
}