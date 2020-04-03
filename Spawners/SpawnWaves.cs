using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnZone{
	L0,
	L1,
	L2A,
	L2B,
	L3A,
	L3B,
	L3C
}


public static class SpawnWaves
{
  
	static SpawnZone[] w1 = new[]{SpawnZone.L1};
	static SpawnZone[] w2a = new[]{SpawnZone.L2A};
	static SpawnZone[] w2b = new[]{SpawnZone.L2B};
	static SpawnZone[] w2ab = new[]{SpawnZone.L2A, SpawnZone.L2B};
	static SpawnZone[] w12a = new[]{SpawnZone.L1, SpawnZone.L2A};
	static SpawnZone[] w12b = new[]{SpawnZone.L1, SpawnZone.L2B};
	static SpawnZone[] w12ab = new[]{SpawnZone.L1, SpawnZone.L2A, SpawnZone.L2B};
	static SpawnZone[] w3a = new[]{SpawnZone.L3A};
	static SpawnZone[] w3b = new[]{SpawnZone.L3B};
	static SpawnZone[] w3c = new[]{SpawnZone.L3C};
	static SpawnZone[] w3ab = new[]{SpawnZone.L3A, SpawnZone.L3B};
	static SpawnZone[] w3ac = new[]{SpawnZone.L3A, SpawnZone.L3C};
	static SpawnZone[] w3bc = new[]{SpawnZone.L3B, SpawnZone.L3C};
	static SpawnZone[] w3abc = new[]{SpawnZone.L3A, SpawnZone.L3B, SpawnZone.L3C};

	static SpawnZone[] w12a3a = new[]{SpawnZone.L1, SpawnZone.L2A, SpawnZone.L3A};
	static SpawnZone[] w12a3b = new[]{SpawnZone.L1, SpawnZone.L2A, SpawnZone.L3B};
	static SpawnZone[] w12a3c = new[]{SpawnZone.L1, SpawnZone.L2A, SpawnZone.L3C};
	static SpawnZone[] w12b3a = new[]{SpawnZone.L1, SpawnZone.L2B, SpawnZone.L3A};
	static SpawnZone[] w12b3b = new[]{SpawnZone.L1, SpawnZone.L2B, SpawnZone.L3B};
	static SpawnZone[] w12b3c = new[]{SpawnZone.L1, SpawnZone.L2B, SpawnZone.L3C};

	static SpawnZone[] w13bc = new[]{SpawnZone.L1, SpawnZone.L3B, SpawnZone.L3C};
	static SpawnZone[] w13abc = new[]{SpawnZone.L1, SpawnZone.L3A, SpawnZone.L3B, SpawnZone.L3C};
	static SpawnZone[] w2ab3abc = new[]{SpawnZone.L2A, SpawnZone.L2B, SpawnZone.L3A, SpawnZone.L3B, SpawnZone.L3C};
	static SpawnZone[] w12ab3abc = new[]{SpawnZone.L1, SpawnZone.L2A, SpawnZone.L2B, SpawnZone.L3A, SpawnZone.L3B, SpawnZone.L3C};


	static SpawnZone[][] waveList = {
		w1, w2ab, w3bc, 
		w2a, w2b, w3abc, 
		w12a, w12b, w13bc, 
		w2ab, w13abc, w12ab3abc 
	};
	

	public static SpawnZone[] GetWave(int wave){
		wave = Mathf.Clamp(wave, 0, 120);
		wave = wave % 12;
		return (waveList[wave]);
	}
}
