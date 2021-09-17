using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterfaceSet
{
    /// <summary>
    /// 공격 관련 인터페이스
    /// </summary>
    public interface IAttack
    {
        /// <summary>
        /// 공격을 할 때 시행될 함수
        /// </summary>
        public void Attack();
    }

    /// <summary>
    /// 피해를 받는 인터페이스
    /// </summary>
    public interface IDamaged
    {
        /// <summary>
        /// 피해를 받았을 때 시행될 함수
        /// </summary>
        public void Damaged(float damage, Vector3 hitPoint, Vector3 hitNormal);
    }


}
