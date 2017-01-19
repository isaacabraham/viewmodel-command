open FsXaml
open ViewModule.FSharp
open System.Windows
open System

type Main = XAML<"Main.xaml">

type ViewModel() as vm =
    inherit ViewModule.ViewModelBase()
    let show = vm.Factory.Backing(<@ vm.Show @>, false)
    
    member private __.Show with get() = show.Value and set(v) = show.Value <- v
    member this.DependentCommand = 
        this.Factory.CommandSyncChecked(
            (fun _ -> MessageBox.Show "Dependent!" |> ignore),
            (fun _ -> this.Show),
            [ <@ this.Show @> ])
    member this.MainCommand =
        this.Factory.CommandSync(fun _ -> this.Show <- true)
            // Not needed anymore
            // vm.DependentCommand.RaiseCanExecuteChanged())

[<STAThread>]
[<EntryPoint>]
let main _ =
    let window = Main(DataContext = ViewModel())
    let app = Application().Run(window)
    app