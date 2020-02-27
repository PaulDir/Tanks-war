using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆炸效果，爆炸效果的销毁
/// </summary>
public class DestroyFotTime : MonoBehaviour {

    public float delayTime; // 爆炸效果延迟销毁时间

	void Start () {
        Destroy(this.gameObject, delayTime); // 爆炸效果，延迟销毁
	}
}
