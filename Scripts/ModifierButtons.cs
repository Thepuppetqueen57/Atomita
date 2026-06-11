using Godot;
using System;

public partial class ModifierButtons : Button {
    [Export] public string ParticleType = "Electron";
    // 1 for plus, 0 for minus
    [Export] public int PlusOrMinus = 1;

    // Define the maximum capacity for each electron shell
    private readonly int[] _shellCapacities = new int[] { 2, 8, 18, 32, 32, 18, 8 }; 

    private const float BaseRadius = 1.0f;
    private const float RadiusStep = 0.5f;

    public override void _Ready() {
        Pressed += OnButtonPressed;
    }

    public void OnButtonPressed() {
        if (PlusOrMinus == 1 && ParticleType == "Electron") {
            AddElectronToNextAvailableShell();
        }
    }

    private void AddElectronToNextAvailableShell() {
        var electron = GD.Load<PackedScene>("res://Prefabs/Electron.tscn").Instantiate();

        for (int i = 0; i < _shellCapacities.Length; i++) {
            string shellNodeName = $"Shell{i + 1}";
            Node shellNode = GetNodeOrNull($"/root/3DView/Atom/Electrons/{shellNodeName}");

            if (shellNode == null) {
                GD.PrintErr($"Expected scene node '{shellNodeName}' missing!");
                electron.QueueFree();
                return;
            }

            // 3. Check if this shell has room
            if (shellNode.GetChildCount() < _shellCapacities[i]) {
                if (electron is Electrons electronScript) {
                    electronScript.CenterNode = GetNode<Node3D>("/root/3DView/Atom/Nucleus");

                    electronScript.RotationRadius = BaseRadius + (i * RadiusStep);
                }

                shellNode.AddChild(electron);
                return; 
            }
        }

        // If the loop finishes, it means all defined shells are completely full
        GD.Print("All electron shells are full!");
        electron.QueueFree(); 
    }
}