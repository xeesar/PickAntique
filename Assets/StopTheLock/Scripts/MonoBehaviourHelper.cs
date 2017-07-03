
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/





using UnityEngine;
using System.Collections;

namespace AppAdvisory.StopTheLock
{
	public class MonoBehaviourHelper : MonoBehaviour 
	{
		private Player _player;
		public Player player
		{
			get
			{
				if (_player == null)
					_player = FindObjectOfType<Player> ();

				return _player;
			}
		}

//		private DotPosition _targetSign;
//		public DotPosition targetSign
//        {
//			get
//			{
//                _targetSign = signsHolder.TargetSign;
//				return _targetSign;
//			}
//		}

        private SignsHolder _holder;
        public SignsHolder signsHolder {
            get
            {
                if (_holder == null)
                    _holder = FindObjectOfType<SignsHolder>();

                return _holder;
            }
        }

		private BonusManager _bonusManager;
		public BonusManager bonusManager {
			get
			{
				if (_bonusManager == null)
					_bonusManager = FindObjectOfType<BonusManager>();

				return _bonusManager;
			}
		}

		private TimeBarScript _TimeBarScript;
		public TimeBarScript TimeBarScript {
			get
			{
				if (_TimeBarScript == null)
					_TimeBarScript = FindObjectOfType<TimeBarScript>();

				return _TimeBarScript;
			}
		}

        private GameManager _gameManager;
		public GameManager gameManager
		{
			get
			{
				if (_gameManager == null)
					_gameManager = FindObjectOfType<GameManager> ();

				return _gameManager;
			}
		}

		private SoundManager _soundManager;
		public SoundManager soundManager
		{
			get
			{
				if (_soundManager == null)
					_soundManager = FindObjectOfType<SoundManager> ();

				return _soundManager;
			}
		}

		private ColorManager _colorManager;
		public ColorManager colorManager
		{
			get
			{
				if (_colorManager == null)
					_colorManager = FindObjectOfType<ColorManager> ();

				return _colorManager;
			}
		}
	}
}