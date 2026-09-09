# Link Catalogue

A C# console coursework project for maintaining a pipe-delimited link list. Commands load, list, add, remove and open entries.

Build `MJU23v_DTP_T2.sln` and run from the build-output directory so the relative `links` path resolves. Enter `help` for commands. The project targets .NET 6; the existing license is retained in `LICENSE.txt`.

The file format contains five pipe-delimited fields: category, group, name, description and URL. Entries are loaded at startup; `save` writes the same format. Missing command arguments are rejected, and only HTTP or HTTPS links can be opened. Field text must not contain pipe characters. Use copies of the sample files when exploring it.
