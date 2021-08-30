using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerDir : MonoBehaviour
{
    public Transform player;
    RectTransform rectTr;
    void Start()
    {
        rectTr = GetComponent<RectTransform>();
    }
    void Update()
    {
        var result = player.position - Camera.main.transform.position;

        //// x랑 z값빼서 x랑 y 값으로 만들기
        //Vector2 playerDir = result.Vec3ToVec2();

        //// x y 좌표평면 상에서 최대 절대값을 -0.5 ~ 0.5 사이로
        //playerDir = playerDir.MapNormal();

        //// -0.5 ~ 0.5 => 0 ~ 1
        //playerDir += new Vector2(0.5f, 0.5f);
        //------------
        //// x랑 z값빼서 x랑 y 값으로 만들기
        //Vector2 playerDir = player.forward.Vec3ToVec2();

        //// x y 좌표평면 상에서 값 -1 ~ 1 => 0 ~ 2로
        //playerDir = (playerDir + Vector2.one);

        //// 0 ~ 2 => 0 ~ 1
        //playerDir *= 0.5f;
        //------------

        //Vector2 playerDir = result.Vec3ToVec2();
        //playerDir.Normalize();
        var playerDir = player.forward.Vec3ToVec2();
        
        if (Mathf.Abs(playerDir.x) > 0.5f)
        {
            playerDir.y = playerDir.y * (1 / Mathf.Abs(2 * playerDir.x));
            playerDir.x = Mathf.Sign(playerDir.x) * 0.5f;
        }

        if (Mathf.Abs(playerDir.y) > 0.5f)
        {
            playerDir.x = playerDir.x * (1 / Mathf.Abs(2 * playerDir.y));
            playerDir.y = Mathf.Sign(playerDir.y) * 0.5f;
        }

        playerDir += new Vector2(0.5f, 0.5f);
        rectTr.anchorMin = playerDir;
        rectTr.anchorMax = playerDir;
    }
}
public static class myext
{
    public static Vector2 Vec3ToVec2(this Vector3 vec3)
    {
        var vec2 = new Vector2(vec3.x, vec3.z);
        return vec2;
    }
    public static Vector2 MapNormal(this Vector2 vec2)
    {
        vec2.x = CutValue(vec2.x);
        vec2.y = CutValue(vec2.y);
        return vec2;

        float CutValue(float v)
        {
            return v > 0.5f ? 0.5f : v < -0.5f ? -0.5f : v;
        }
    }
}