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

Quick start — CLI
````````

What the UI does
- `OptimizationForm` shows explanatory text and two buttons:
  - `No, launch the game without changing any settings` — closes dialog with `DialogResult.No`.
  - `Yes, change the settings and launch the game` — closes dialog with `DialogResult.Yes` (the calling code should apply the registry change and then start the game).
- Resource strings are in `OptimizationForm.resx`.

Security & permissions
- Applying optimizations may require writing to the registry; ensure the launcher runs with appropriate privileges.
- Do not commit secrets or large binaries. Use `.gitignore` and Git LFS for large assets.

Recommended repository contents
- `README.md` (this file)
- `.gitignore` (Visual Studio / .NET defaults)
- `LICENSE` (e.g., MIT)
- `src/` or project folder containing the `.csproj`, `OptimizationForm.cs`, `OptimizationForm.Designer.cs`, `OptimizationForm.resx`, etc.
- Optional: `docs/`, `assets/` (screenshots), `tests/`

Development notes
- Consider renaming the class/file `OptimizationForm` → `OptimizationForm` for correct spelling; use __Refactor > Rename...__ in Visual Studio to update references safely.
- Ensure `OptimizationForm.resx` Build Action is set to `Embedded Resource` (usually handled automatically).

Contributing
- Fork, create a feature branch, open a pull request.
- Follow .NET 8, C# coding conventions.
- Include unit/integration tests where applicable.

Troubleshooting
- "SDK not found" — install .NET 8 SDK and restart the IDE/terminal.
- "Access denied" when applying settings — run the launcher elevated or adjust the calling code to prompt for elevation before registry writes.
- Missing resources — verify `.resx` entries and that files are included in the project.

License
- Add a `LICENSE` file (MIT recommended) and replace this section with the chosen license name.

Contact / metadata
- Project: `GameLauncher`
- Main form: `OptimizationForm` (see `OptimizationForm.cs`, `OptimizationForm.Designer.cs`, `OptimizationForm.resx`)
- Target framework: .NET 8
