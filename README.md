# GameLauncher (PGMMV Player - Launcher)

Short description
- A .NET 8 WinForms launcher for the PGMMV player that presents an optimization prompt before launching the game. Includes `OptimizationForm` (dialog), resources, and UI code.

Key features
- Prompt to apply a Windows graphics optimization (registry entry) before launching.
- Simple Yes/No flow: `Yes` applies changes and launches, `No` launches without changes.
- Built with .NET 8 and Windows Forms.

Requirements
- .NET 8 SDK
- Windows (WinForms desktop app)
- Visual Studio 2026 (recommended) or the `dotnet` CLI
- Administrator privileges when applying system-level registry changes

Quick start — Visual Studio
1. Open the solution in Visual Studio 2026.
2. Make sure the correct startup project is selected.
3. Build: use __Build > Build Solution__.
4. Run: use __Debug > Start Debugging__ or __Debug > Start Without Debugging__.

Contact / metadata
- Project: `GameLauncher`
- Main form: `OptimizationForm` (see `OptimizationForm.cs`, `OptimizationForm.Designer.cs`, `OptimizationForm.resx`)
- Target framework: .NET 8
