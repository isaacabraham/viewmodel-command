open FsXaml
open ViewModule.FSharp
open System.Windows
open System

type Main = XAML<"Main.xaml">

type ViewModel() as vm =
    inherit ViewModule.ViewModelBase()
    let mutable show = false

    // This needs to be let bound, since you're referring to it via the property later
    // If it's created as the property's getter, it's a _new instance_ each time you 
    // call vm.DependentCommand, which means the original (bound to view) instance
    // never gets notified
    let dc = vm.Factory.CommandSyncChecked(
                (fun _ -> MessageBox.Show "Dependent!" |> ignore),
                fun _ -> show)

    member __.DependentCommand = dc        
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