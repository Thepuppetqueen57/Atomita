using Godot;
using System;

public partial class ModifierButtons : Button {
    [Export] public string ParticleType = "Electron";
    // 1 for plus 0 for minus
    [Export] public int PlusOrMinus = 1;

    public override void _Ready() {
        Pressed += OnButtonPressed;
    }

    public void OnButtonPressed() {
        if (PlusOrMinus == 1) {
            if (ParticleType == "Electron") {
                var Electron = GD.Load<PackedScene>("res://Prefabs/Electron.tscn").Instantiate();
                if (GetNode("/root/3DView/Atom/Electrons/Shell1").GetChildCount() <= 2) {
                    var StartingAngle = GetNode("/root/3DView/Atom/Electrons/Shell1").GetChildCount() * 120.0f;
                    if (Electron is Electrons electronScript) {
                        electronScript.CenterNode = GetNode<Node3D>("/root/3DView/Atom/Nucleus");
                        electronScript.RotationRadius = 1.0f;
                        electronScript.Speed = 2.0f;
                        electronScript.StartingAngle = StartingAngle;
                    }
                    GetNode("/root/3DView/Atom/Electrons/Shell1").AddChild(Electron);
                }
            }
        }
    }
}
