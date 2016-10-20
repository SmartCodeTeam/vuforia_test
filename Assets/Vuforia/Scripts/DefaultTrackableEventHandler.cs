/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
    
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
//            if (newStatus == TrackableBehaviour.Status.DETECTED ||
//                newStatus == TrackableBehaviour.Status.TRACKED ||
//                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
//            {
//				//加えた(2016/8/10)
//				DataManager.Instance.blockStatus.Remove(name);
//				DataManager.Instance.blockStatus.Add(name,1);
//				//ここまで
//                OnTrackingFound();
//            }
			//if を2パートに
			if (newStatus == TrackableBehaviour.Status.DETECTED)
			{
				//加えた(2016/8/10)
				DataManager.Instance.blockStatus.Remove(name);//ディクショナリのAddは上書き機能がない為、Removeしてる。(インデクサならok)
				DataManager.Instance.blockStatus.Add(name,1);
				DataManager.Instance.NewDetected=name;
				//ここまで
				OnTrackingFound();
			}else if(newStatus == TrackableBehaviour.Status.TRACKED ||
				newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			{
				DataManager.Instance.blockStatus.Remove(name);
				DataManager.Instance.blockStatus.Add(name,1);
				DataManager.Instance.NewDetected=name;
				//ここまで
				OnTrackingFound();
			}
			//2パートにしたの終わり
            else
            {
				//加えた(2016/8/10)
				DataManager.Instance.blockStatus.Remove(name);
				DataManager.Instance.blockStatus.Add(name,0);
				DataManager.Instance.NewLost=name;
				//ここまで
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

//            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

//            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
    }
}
