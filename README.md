# SamllHax.MapleSyrup
Because there were not enough unfinished custom MapleStory clients, I decided to make my own.

The goal is to have single, clean, cross-platform application that does everything to achieve a single player Maple Story experience, with no or minimal setup and no proprietary code/data.
Ideally this client would directly use provided wz files, but I might go with a different format that requires a conversion first.
Currently I just use files dumped by <a href="https://github.com/Xterminatorz/WZ-Dumper">WZ.Dumper</a>

Everything is written from scratch, but I use <a href="https://github.com/Libre-Maple/LibreMaple-Client">LibreMaple</a> as an inspiration when I get stuck.

Currently the client partially renders maps (no backgrounds) and allows traveling using non scripted portals.

Client is written in .Net Core 6 and uses SkiaSharp for rendering 2D graphics (with OpenTK creating a native window and providing OponGL context). What lib (if any) to use for getting wz data is up in the air.

If this project ever goes anywhere, I'll try implementing network CO-OP mode (similarly to how multiplayer works in Terraria or Stardew Valley) and maybe just maybe more traditional client/server. For now compatibility with actual MS servers and having any security measures is not even in the picture.

License might change, but currently it's WTFPL.
