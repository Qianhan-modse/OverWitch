using System;
using System.Collections;
using System.Collections.Generic;
using Entitying;
using UnityEngine;

public interface IChunkProvider
{
    public Chunk getLoadedChunk(int var1,int var2);
    public Chunk provideChunk(int var1,int var2);
    bool tick();
    String makeString();
    bool isChunkGeneratedAt(int var1,int var2);
}
