using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectCounting
{
    // 각 몬스터 오브젝트 소환갯수를 저장하는 변수
    public static int zombieSpwanCount = 10;
    public static int spiderSpwanCount = 0;
    public static int clutchSpwanCount = 0;
    public static int MovidicSpwanCount = 0;

    public static List<int> SpwanCounting(int _stage)
    {
        List<int> spawnCount = new List<int>();

        zombieSpwanCount += 3;
        if (_stage >= 3 && _stage % 3 == 0)
        {
            spiderSpwanCount += 3;
        }
        if (_stage >= 5 && _stage % 5 == 0)
        {
            clutchSpwanCount += 2;
        }
        if (_stage % 10 == 0)
        {
            MovidicSpwanCount = 1;
        }
        else
        {
            MovidicSpwanCount = 0;
        }

        spawnCount.Add(zombieSpwanCount);
        spawnCount.Add(spiderSpwanCount);
        spawnCount.Add(clutchSpwanCount);
        spawnCount.Add(MovidicSpwanCount);

        return spawnCount;
    }




}
