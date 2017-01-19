open FsXaml
open ViewModule.FSharp
open System.Windows
open System

type Main = XAML<"Main.xaml">

type ViewModel() as vm =
    inherit ViewModule.ViewModelBase()
    let mutable show = false
    member __.DependentCommand =
        vm.Factory.CommandSyncChecked(
            (fun _ -> MessageBox.Show "Dependent!" |> ignore),
            fun _ -> show)
    member __.MainCommand =
        vm.Factory.CommandSync(fun _ ->
            show <- true
            // I would expect DependentCommand to become available after this next call.
            vm.DependentCommand.RaiseCanExecuteChanged())

[<STAThread>]
[<EntryPoint>]
let main _ =
    let window = Main(DataContext = ViewModel())
    let app = Application().Run(window)
    app