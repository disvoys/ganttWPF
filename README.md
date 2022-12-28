# ganttWPF
## _Because I did not find a free solution_

## Images
![Screenshot of the UI](/Capture.PNG "Image")
## How that works

- XAML
```sh
<Page xmlns:UI_perso="clr-namespace:appCore.components.UI_perso"  x:Class="appCore.components.macrosPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
      xmlns:local="clr-namespace:appCore.components"
      mc:Ignorable="d" 
      xmlns:materialDesign="http://materialdesigninxaml.net/winfx/xaml/themes"
      d:DesignHeight="1000" d:DesignWidth="1000" Initialized="Page_Initialized"
      Title="macrosPage" Loaded="Page_Loaded">
    <Page.Resources>
        <SolidColorBrush x:Key="violetClair" Color="Black" />
    </Page.Resources>
    <Grid>
        <UI_perso:gantt x:Name="myGantt"/> 
    </Grid>
</Page>
```

- C# | Sample to add 3 new tasks
```sh
   private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            gantt.tasks myTask = new gantt.tasks();
            myTask.name = "test nouvelle tache";
            myTask.dayStart = DateTime.Now;
            myTask.dayEnd = DateTime.Now.AddDays(10);
            myTask.statusPercent = 0.3;
            myGantt.AddTask(myTask);

            gantt.tasks myTask_ = new gantt.tasks();
            myTask_.name = "test nouvelle tache 2";
            myTask_.dayStart = new DateTime(2022,12,13);
            myTask_.dayEnd = DateTime.Now.AddDays(5);
            myTask_.statusPercent = 0.5;
            myGantt.AddTask(myTask_);

            gantt.tasks myTask__ = new gantt.tasks();
            myTask__.name = "test nouvelle tache 3";
            myTask__.dayStart = DateTime.Now.AddDays(-10);
            myTask__.dayEnd = DateTime.Now.AddDays(3);
            myTask__.statusPercent = 1;
            myGantt.AddTask(myTask__);       
        }
```
