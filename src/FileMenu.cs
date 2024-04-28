using Godot;
using System;

public partial class FileMenu : PopupMenu
{
    [Export]
    FileDialog fileDialog;

    override public void _Ready()
    {
        this.IdPressed += on_id_pressed;
    }

    private void on_id_pressed(long id)
    {
        GD.Print("ID Pressed: " + id);
        if(id == 0) {
            fileDialog.Show();
        }
    }


    public void Test() {
        GD.Print("Test");
    }
}
