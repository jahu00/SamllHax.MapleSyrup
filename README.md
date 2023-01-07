# SamllHax.MapleSyrup
Because there were not enought unfinisjed cusomt MapleStory clients, I decided to make my own.

The goal is to have single, clean, cross-platform application that does everything to achieve a single player Maple Story expirience, whit no or minimal setup.
Ideally this client would directly use provided wz files, but I might go with a different format that requires a conversion first.
Currently I just use files dumped by <a href="https://github.com/Xterminatorz/WZ-Dumper">WZ.Dumper</a>

Everything is written from scratch, but I use <a href="https://github.com/Libre-Maple/LibreMaple-Client">LibreMaple</a> as an inspiration.
Currently this client is only able to render a map to a png file.

Other than wanting to use C#, all libraries and technologies are subject to change. Currently it's .Net Core 6, OpenTK and SkiaSharp.
If implementing without a game engine proves to be too daunting, I might move to Godot with Mono.

If this project ever goes anywhere, I'll try implementing network CO-OP mode (similarly to how multiplayer works in Terraria or Stardew Valley).

License might change, but currently it's WTFPL.
