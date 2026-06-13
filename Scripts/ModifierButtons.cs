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
        if (ParticleType == "Electron") {
            if (PlusOrMinus == 1 && Globals.ElectronCount < 118) {
                Globals.ElectronCount++;
                UpdateVisualElectrons();
            } else if (PlusOrMinus == 0 && Globals.ElectronCount > 0) {
                Globals.ElectronCount--;
                UpdateVisualElectrons();
            }
        } else if (ParticleType == "Proton") {
            if (PlusOrMinus == 1) {
                AddProton();
                Globals.ProtonCount++;
            } else if (PlusOrMinus == 0 && Globals.ProtonCount > 1) {
                RemoveProton();
                Globals.ProtonCount--;
            }
        } else if (ParticleType == "Neutron") {
            if (PlusOrMinus == 1) {
                AddNeutron();
                Globals.NeutronCount++;
            } else if (PlusOrMinus == 0 && Globals.NeutronCount > 0) {
                RemoveNeutron();
                Globals.NeutronCount--;
            }
        }
    }

    private void AddProton() {
        var proton = GD.Load<PackedScene>("res://Prefabs/Proton.tscn").Instantiate() as Node3D;
        if (proton == null) return;

        float theta = GD.Randf() * Mathf.Tau;
        float phi = Mathf.Acos(2.0f * GD.Randf() - 1.0f);

        float baseRadius = 0.1f;
        float volumeFactor = (float)Math.Cbrt(Globals.ProtonCount + Globals.NeutronCount + 2) * 0.1f;
        float finalRadius = baseRadius + volumeFactor;

        float x = finalRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = finalRadius * Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = finalRadius * Mathf.Cos(phi);

        proton.Position = new Vector3(x, y, z);

        GetNode<Node3D>("/root/3DView/Atom/Nucleus/Protons").AddChild(proton);
    }

    private void RemoveProton() {
        Node3D nucleus = GetNode<Node3D>("/root/3DView/Atom/Nucleus/Protons");
        if (Globals.ProtonCount > 0) {
            Node lastProton = nucleus.GetChild(nucleus.GetChildCount() - 1);
            nucleus.RemoveChild(lastProton);
            lastProton.QueueFree();
        }
    }

    private void AddNeutron() {
        var neutron = GD.Load<PackedScene>("res://Prefabs/Neutron.tscn").Instantiate() as Node3D;
        if (neutron == null) return;

        float theta = GD.Randf() * Mathf.Tau;
        float phi = Mathf.Acos(2.0f * GD.Randf() - 1.0f);

        float baseRadius = 0.1f;
        float volumeFactor = (float)Math.Cbrt(Globals.ProtonCount + Globals.NeutronCount + 2) * 0.1f;
        float finalRadius = baseRadius + volumeFactor;

        float x = finalRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = finalRadius * Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = finalRadius * Mathf.Cos(phi);

        neutron.Position = new Vector3(x, y, z);

        GetNode<Node3D>("/root/3DView/Atom/Nucleus/Neutrons").AddChild(neutron);
    }

    private void RemoveNeutron() {
        Node3D nucleus = GetNode<Node3D>("/root/3DView/Atom/Nucleus/Neutrons");
        if (Globals.NeutronCount > 0) {
            Node lastNeutron = nucleus.GetChild(nucleus.GetChildCount() - 1);
            nucleus.RemoveChild(lastNeutron);
            lastNeutron.QueueFree();
        }
    }

    private void UpdateVisualElectrons() {
        int[] targetConfig = GetBohrConfiguration(Globals.ElectronCount);

        for (int i = 0; i < targetConfig.Length; i++) {
            string shellNodeName = $"Shell{i + 1}";
            Node shellNode = GetNodeOrNull($"/root/3DView/Atom/Electrons/{shellNodeName}");

            if (shellNode == null) {
                GD.PrintErr($"Expected scene node '{shellNodeName}' missing!");
                continue;
            }

            int currentCount = shellNode.GetChildCount();
            int targetCount = targetConfig[i];
            while (currentCount < targetCount) {
                var electron = GD.Load<PackedScene>("res://Prefabs/Electron.tscn").Instantiate();
                if (electron is Electrons electronScript) {
                    electronScript.CenterNode = GetNode<Node3D>("/root/3DView/Atom/Nucleus");
                    electronScript.RotationRadius = BaseRadius + (i * RadiusStep);
                }
                shellNode.AddChild(electron);
                currentCount++;
            }

            while (currentCount > targetCount) {
                Node electronToRemove = shellNode.GetChild(currentCount - 1);
                shellNode.RemoveChild(electronToRemove);
                electronToRemove.QueueFree();
                currentCount--;
            }

            RearrangeShellElectrons(shellNode);
        }
    }

    private void RearrangeShellElectrons(Node shellNode) {
        int totalElectrons = shellNode.GetChildCount();
        if (totalElectrons == 0) return;

        float angleStep = Mathf.Tau / totalElectrons;

        for (int i = 0; i < totalElectrons; i++) {
            Node child = shellNode.GetChild(i);

            if (child is Electrons eScript) {
                eScript.CurrentAngle = i * angleStep;
            }
        }
    }

    private int[] GetBohrConfiguration(int totalElectrons) {
        int[] shells = new int[7];
        if (totalElectrons <= 0) return shells;

        // Standard Aufbau filling order by subshells (n, l)
        // Format: (shellIndex, subshellCapacity)
        var subshells = new (int shell, int capacity)[] {
            (0, 2),  // 1s
            (1, 2),  // 2s
            (1, 6),  // 2p
            (2, 2),  // 3s
            (2, 6),  // 3p
            (3, 2),  // 4s
            (2, 10), // 3d
            (3, 6),  // 4p
            (4, 2),  // 5s
            (3, 10), // 4d
            (4, 6),  // 5p
            (5, 2),  // 6s
            (4, 14), // 4f
            (5, 10), // 5d
            (6, 6),  // 6p
            (6, 2),  // 7s
            (5, 14), // 5f
            (6, 10), // 6d
            (6, 6)   // 7p
        };

        int remaining = totalElectrons;
        foreach (var subshell in subshells) {
            int take = Mathf.Min(remaining, subshell.capacity);
            shells[subshell.shell] += take;
            remaining -= take;
            if (remaining <= 0) break;
        }

        return shells;
    }
}