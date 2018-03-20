#pragma once

extern "C" __declspec(dllimport) char* __cdecl cdgeterror();
extern "C" __declspec(dllimport) bool __cdecl cdshowdisplay(int mode);
extern "C" __declspec(dllimport) void __cdecl cdsetimagedir(char* path);
extern "C" __declspec(dllimport) void __cdecl cdsenddata(int message, int itemid, char* name, int quantity, char* total, char* taxtotal, char* extra);
extern "C" __declspec(dllimport) bool __cdecl