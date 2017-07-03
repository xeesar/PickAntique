using UnityEngine;
using System.Collections;
using System;

namespace AppAdvisory.StopTheLock
{
	public class Player : MonoBehaviourHelper
	{
		public float speed = 1f;
		public float defaultSpeed = 1f;
		public float bonusSpeed = 7f;
		public int currentSectorIndex;

		public bool firstMove;

		float time = 2.7f;

		int direction = 1;

		private GameObject sign;

		//		Vector2 currentDotPosition
		//		{
		//			get
		//			{
		//				return targetSign.GetPosition ();
		//			}
		//		}

		[SerializeField] private Transform playerTransform;

		public void IncreaseSpeed () {
			float coeff1 = 3f, coeff2 = 7f;
			if (ProgressManager.progressManager.chestGrade == Assets.Scripts.Enums.ChestGrades.Base) {
				coeff1 = 3f;
				coeff2 = 7f;
			}
			if (ProgressManager.progressManager.chestGrade == Assets.Scripts.Enums.ChestGrades.Rare) {
				coeff1 = 3f;
				coeff2 = 7f;
			}
			if (ProgressManager.progressManager.chestGrade == Assets.Scripts.Enums.ChestGrades.Epic) {
				coeff1 = 3f;
				coeff2 = 7f;
			}
			if (gameManager.gameManager.score >= 30) {
				coeff1 = 2f;
				coeff2 = 50f;
				defaultSpeed = 1.8f;
			} 
			speed = (float) (defaultSpeed * (1 + (Math.Sin (gameManager.score) + (gameManager.score / coeff1)) / coeff2));
			print ("Speed: " + speed);
		}
		public Transform GetTransform()
		{
			return playerTransform;
		}

		public float GetRotation()
		{
			return transform.eulerAngles.z;
		}

		void Awake()
		{
			firstMove = true;
			direction = 1;
		}

		void Update()
		{
			if (!gameManager.gameIsStarted)
			{
				firstMove = true;
				StopAllCoroutines ();
				return;
			}
			if (gameManager.superBonus) {
				StopAllCoroutines ();
				StartCoroutine (_StartTheMove());
			}
//			var segmentIndex = GetCurrentsegmentIndex();
//			if (segmentIndex != signsHolder.currentIndex)
//			{
//				signsHolder.currentIndex = segmentIndex;
//				signsHolder.HighlightCurrent();
//			}
			if (gameManager.missed){
				StartTheMove();
				gameManager.missed = false;
			}

			if (Input.GetMouseButtonDown (0)) //&& !gameManager.isGameOver && (!gameManager.isSuccess || gameManager.bonusGame) && !TimeBarScript.isMoving)
			{
				Debug.Log ("Clicked");
				if (Input.mousePosition.y > Screen.height * 0.9)
					return;

				if (firstMove)
				{
					if (signsHolder.signs[signsHolder.targetSignIndexes[0]].isLeftOfScreen())
						direction = -1;
					else
						direction = +1;

					StartTheMove ();

					firstMove = false;
					return;
				}

				Vector2 myPos = playerTransform.position;

				//float dist = Vector2.Distance (currentDotPosition, myPos);
				currentSectorIndex = GetCurrentsegmentIndex();


				if (signsHolder.CheckForTarget(currentSectorIndex)) //32
				{
					signsHolder.InitFireworks (currentSectorIndex);
					signsHolder.MoveCountTrail (currentSectorIndex);
					SetRandomTarget ();
					gameManager.MoveDone ();
					if (!gameManager.superBonus) {
						signsHolder.SetRandomBonus ();
					}

					StartTheMove ();
				}
				else
				{
					if (bonusManager.CheckOnBonus(currentSectorIndex)) {
						soundManager.PlayBonus ();
						Debug.Log ("ActivateBonus");
						bonusManager.ActivateBonus (currentSectorIndex);
						//StopCoroutine ("CountTrail");
						//StartCoroutine (signsHolder.CountTrail(currentSectorIndex));
						SetRandomTarget ();
						if (!gameManager.superBonus) {
							signsHolder.SetRandomBonus ();
						}
						StartTheMove ();
					} else {
						Debug.Log ("MissDone");
						if (!gameManager.superBonus && !bonusManager.CheckOnActiveBonus(BonusTypes.Freeze)) {
							signsHolder.DecreaseCounter (currentSectorIndex);
						}
						SetRandomTarget ();
						if (!gameManager.superBonus && !gameManager.isLastChance) {
							signsHolder.SetRandomBonus ();
						}
						gameManager.MissDone();
					}

					//gameManager.GameOver ();
				}
			}
		}

		private void SetRandomTarget () {
			bonusManager.ResetBonuses ();
			signsHolder.SetRandomSign ();
		}

		public void ChangePlayerToBonus (){
			transform.Find("Projectile/Trail").gameObject.SetActive (false);
			transform.Find("Projectile/BonusTrail").gameObject.SetActive (true);
		}

		int GetCurrentsegmentIndex() {
			var sectorSize = 360 / signsHolder.signs.Count;
			var currSectorIndex = (360 - (playerTransform.eulerAngles.z % 360) + sectorSize / 2) / sectorSize;
			return (int)(Math.Floor(currSectorIndex) % signsHolder.signs.Count);
		}

		void StartTheMove()
		{
			gameManager.levelTopScreen.text = "";
			//if (gameManager.isSuccess && !gameManager.bonusGame)
			//{
			//StopAllCoroutines ();
			//return;
			//}

			direction *= -1;

			StopAllCoroutines ();
			StartCoroutine (_StartTheMove());
		}

		IEnumerator _StartTheMove()
		{
			float currSpeed = speed;
			if (gameManager.superBonus) {
				currSpeed = bonusSpeed;
			} else {
				currSpeed = speed;
			}
			while (!gameManager.isGameOver)
			{
				float t0 = transform.rotation.eulerAngles.z;
				float t1 = transform.rotation.eulerAngles.z + direction * 360f;
				float timer = 0;

				while (timer <= time)
				{
					timer += Time.deltaTime * currSpeed;

					float f = Mathf.Lerp (t0, t1, timer / time);

					Vector3 rot = Vector3.forward * f;

					transform.eulerAngles = rot;

					Vector2 myPos = playerTransform.position;

					if (gameManager.isGameOver)
						break;

					yield return null;
				}
			}
		}
	}
}