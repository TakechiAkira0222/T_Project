using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakechiEngine.PUN.CustomProperties
{
    public class TakechiPunCustomProperties : TakechiPunCallbacks
    {
        #region setPropertiesFunction
        /// <summary>
        /// ニックネームを付ける
        /// </summary>
        /// <param name="nickName"> 設定したい自分の名前 </param>
        protected void SetMyNickName(string nickName)
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = nickName;

                Debug.Log($" LocalPlayer.NickName : <color=blue>{nickName}</color> <color=green> to set.</color>");
            }
        }

        #endregion

        #region UpdateCustomRoomProperties
        /// <summary>
        /// CurrentRoomCustomProperties to set.
        /// </summary>
        /// <param name="customRoomProperties"> 更新したい状態のプロパティー </param>
        protected void setCurrentRoomCustomProperties(ExitGames.Client.Photon.Hashtable customRoomProperties)
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
                Debug.Log($" setCurrentRoomCustomProperties : <color=green> current room custom properties to set.</color>");
            }
        }
        /// <summary>
        /// CurrentRoomCustomProperties to set.
        /// </summary>
        /// <remarks>　部屋のカスタムプロパティーに存在するかどうかを見て追記もしくは書き換えを行います。　</remarks>>
        /// <param name="key"> プロパティーの鍵　</param>
        /// <param name="o">　変更内容のオブジェクト型 </param>
        protected void setCurrentRoomCustomProperties(string key, object o)
        {
            if ( PhotonNetwork.InRoom)
            {
                object temp = null;
                ExitGames.Client.Photon.Hashtable hastable = new ExitGames.Client.Photon.Hashtable();

                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(key, out temp))
                {
                    hastable[key] = o;
                    Debug.Log($" setCurrentRoomCustomProperties[<color=orange>{key}</color>] : {o} <color=green>to set.</color>");
                }
                else
                {
                    hastable.Add(key, o);
                    Debug.Log($" setCurrentRoomCustomProperties[<color=orange>{key}</color>] : {o} <color=green>to add.</color>");
                }

                PhotonNetwork.CurrentRoom.SetCustomProperties(hastable);
            }
        }
        /// <summary>
        /// CurrentRoomCustomProperties to set.
        /// </summary>
        /// <remarks>　部屋のカスタムプロパティーに存在するかどうかを見て追記もしくは書き換えを行います。　</remarks>>
        /// <param name="customRoomProperties">　現在のプロパティー </param>
        /// <param name="key"> プロパティーの鍵　</param>
        /// <param name="o">　変更内容のオブジェクト型 </param>
        protected void setCurrentRoomCustomProperties(ExitGames.Client.Photon.Hashtable customRoomProperties, string key, object o)
        {
            if (PhotonNetwork.InRoom)
            {
                object temp = null;
                if (customRoomProperties.TryGetValue(key, out temp))
                {
                    customRoomProperties[key] = o;
                    Debug.Log($" setCurrentRoomCustomProperties[<color=orange>{key}</color>] : {o} <color=green>to set.</color>");
                }
                else
                {
                    customRoomProperties.Add(key, o);
                    Debug.Log($" setCurrentRoomCustomProperties[<color=orange>{key}</color>] : {o} <color=green>to add.</color>");
                }

                PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
            }
        }

        #endregion

        #region setLocalPlayerCustomProperties

        /// <summary>
        /// LocalPlayerCustomProperties to set.
        /// </summary>
        /// <param name="customRoomProperties"> 更新したい状態のプロパティー </param>
        protected void setLocalPlayerCustomProperties(ExitGames.Client.Photon.Hashtable customRoomProperties)
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(customRoomProperties);
                Debug.Log($" setLocalPlayerCustomProperties : <color=green> local player custom properties to set.</color>");
            }
        }
        /// <summary>
        /// LocalPlayerCustomProperties to set.
        /// </summary>
        /// <remarks>　自身のカスタムプロパティに存在するかどうかを見て追記もしくは書き換えを行います。　</remarks>>
        /// <param name="key"> プロパティーの鍵　</param>
        /// <param name="o">　変更内容のオブジェクト型 </param>
        protected void setLocalPlayerCustomProperties(string key, object o)
        {
            if (PhotonNetwork.IsConnected)
            {
                object temp = null;
                ExitGames.Client.Photon.Hashtable hastable = new ExitGames.Client.Photon.Hashtable();

                if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(key, out temp))
                {
                    hastable[key] = o;
                    Debug.Log($" setLocalPlayerCustomProperties[<color=orange>{key}</color>] : {o} <color=green>to set.</color>");
                }
                else
                {
                    hastable.Add(key, o);
                    Debug.Log($" setCurrentRoomCustomProperties[<color=orange>{key}</color>] : {o} <color=green>to add.</color>");
                }

                PhotonNetwork.LocalPlayer.SetCustomProperties(hastable);
            }
        }
        /// <summary>
        /// LocalPlayerCustomProperties to set.
        /// </summary>
        /// <remarks>　自身のカスタムプロパティに存在するかどうかを見て追記もしくは書き換えを行います。　</remarks>>
        /// <param name="customPlayerProperties">　現在のプロパティ </param>
        /// <param name="key"> プロパティーの鍵　</param>
        /// <param name="o">　変更内容のオブジェクト型 </param>
        protected void setLocalPlayerCustomProperties(ExitGames.Client.Photon.Hashtable customPlayerProperties, string key, object o)
        {
            if (PhotonNetwork.InRoom)
            {
                object temp = null;
                if (customPlayerProperties.TryGetValue(key, out temp))
                {
                    customPlayerProperties[key] = o;
                    Debug.Log($" setCurrentRoomCustomProperties[<color=orange>{key}</color>] : {o} <color=green>to set.</color>");
                }
                else
                {
                    customPlayerProperties.Add(key, o);
                    Debug.Log($" setCurrentRoomCustomProperties[<color=orange>{key}</color>] : {o} <color=green>to add.</color>");
                }

                PhotonNetwork.LocalPlayer.SetCustomProperties(customPlayerProperties);
            }
        }

        #endregion

    }
}
