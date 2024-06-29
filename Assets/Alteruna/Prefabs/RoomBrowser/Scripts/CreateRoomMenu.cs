using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Alteruna
{
	public class CreateRoomMenu : CommunicationBridge
	{
		[Range(0, 30)]
		public int MaxNameLength = 20; // It's recommended to limit the length of RoomName since strings can easily scale up bandwith usage.
		[Range(1, 6)]
		public int MaxPlayers = 6;
		[Range(1, 10)]
		public int StartingDiscards = 4;
		[Range(2, 10)]
		public int StartingHands = 4;
		[Range(2, 60)]
		public int TimeLimit = 4;
		[Range(1, 10)]
		public int Rounds = 4;
		[Range(1, 6)]
		public int MaxJokers = 3;
		[Range(1, 4)]
		public int MoneyMult = 1;
		[Range(0, 100)]
		public int JokerChance = 75;

		[SerializeField] private Image _imageMap;
		[SerializeField] private TMP_Text _textMapTitle;
		[SerializeField] private TMP_Text _textMapInfo;
		[SerializeField] private TMP_Text _textInviteCode;

		[Header("Settings")]

		[SerializeField] private TMP_InputField _inputRoomName;
		[SerializeField] private TMP_InputField _inputMaxPlayers;
		[SerializeField] private TMP_InputField _inputStartingDiscards;
		[SerializeField] private TMP_InputField _inputStartingHands;
		[SerializeField] private TMP_InputField _inputTimeLimit;
		[SerializeField] private TMP_InputField _inputRound;
		[SerializeField] private TMP_InputField _inputMaxJokers;
		[SerializeField] private TMP_InputField _inputMoneyMult;
		[SerializeField] private TMP_InputField _inputJokerChance;
		[SerializeField] private TMP_InputField _inputPassword;
		[SerializeField] private TMP_Dropdown _dropdownGameMode;
		//[SerializeField] private TMP_Dropdown _dropdownScene;
		[SerializeField] private Toggle _toggleHideRoom;
		[SerializeField] private Button _buttonCreateRoom;

		private CustomRoomInfo _customRoomInfo;

		[SerializeField] private GameObject PlayerPrefab;
		[SerializeField] private GameObject Lobby;
		[SerializeField] private GameObject roomBrowser;
		[SerializeField] private GameObject touchBlock;
		[SerializeField] private GameObject Gameplayinfo;

		void Start()
		{	
			_customRoomInfo = new CustomRoomInfo();

			RoomNameChanged(Multiplayer.Me.Name);
			PopulateDropdownWithEnumValues(_dropdownGameMode, typeof(GameMode));
			//PopulateDropdownWithSceneNames(_dropdownScene);
			SetMapInfo();

			Gameplayinfo = GameObject.Find("GameplayInfo");

			_inputRoomName.characterLimit = MaxNameLength;

			_inputRoomName.onValueChanged.AddListener(RoomNameChanged);
			_inputMaxPlayers.onEndEdit.AddListener(delegate{NumberInputChanged(_inputMaxPlayers.text, MaxPlayers, _inputMaxPlayers, 1, 6);});
			_inputStartingDiscards.onEndEdit.AddListener(delegate{NumberInputChanged(_inputStartingDiscards.text, StartingDiscards, _inputStartingDiscards, 2, 10);});
			_inputStartingHands.onEndEdit.AddListener(delegate{NumberInputChanged(_inputStartingHands.text, StartingHands, _inputStartingHands, 2, 10);});
			_inputTimeLimit.onEndEdit.AddListener(delegate{NumberInputChanged(_inputTimeLimit.text, TimeLimit, _inputTimeLimit, 10, 60);});
			_inputRound.onEndEdit.AddListener(delegate{NumberInputChanged(_inputRound.text, Rounds, _inputRound, 1, 10);});
			_inputMaxJokers.onEndEdit.AddListener(delegate{NumberInputChanged(_inputMaxJokers.text, MaxJokers, _inputMaxJokers, 0, 6);});
			_inputMoneyMult.onEndEdit.AddListener(delegate{NumberInputChanged(_inputMoneyMult.text, MaxJokers, _inputMoneyMult, 1, 4);});
			_inputJokerChance.onEndEdit.AddListener(delegate{NumberInputChanged(_inputJokerChance.text, MaxJokers, _inputJokerChance, 0, 100);});

			_dropdownGameMode.onValueChanged.AddListener(GameModeChanged);
			//_dropdownScene.onValueChanged.AddListener(MapChanged);
			_toggleHideRoom.onValueChanged.AddListener(ToggleHideRoom);

			_buttonCreateRoom.onClick.AddListener(Submit);

			Multiplayer.OnRoomCreated.AddListener(CreatedRoom);
		}

		private void ToggleHideRoom(bool value)
		{
			_inputPassword.transform.parent.gameObject.SetActive(!value);
		}

		public void ChangeMaxPlayersValue(int value)
		{
			int maxPlayers = int.Parse(_inputMaxPlayers.text) + value;
			maxPlayers = HandleNumberInput(maxPlayers, 1, 6);
			_inputMaxPlayers.SetTextWithoutNotify(maxPlayers.ToString());
		}

		public void ChangeStartingDiscardsValue(int value)
		{
			int startingDiscards = int.Parse(_inputStartingDiscards.text) + value;
			startingDiscards = HandleNumberInput(startingDiscards, 2, 10);
			_inputStartingDiscards.SetTextWithoutNotify(startingDiscards.ToString());
		}

		public void ChangeStartingHandsValue(int value)
		{
			int startingHands = int.Parse(_inputStartingHands.text) + value;
			startingHands = HandleNumberInput(startingHands, 2, 10);
			_inputStartingHands.SetTextWithoutNotify(startingHands.ToString());
		}

		public void ChangeTimeLimitValue(int value)
		{
			int timeLimit = int.Parse(_inputTimeLimit.text) + value;
			timeLimit = HandleNumberInput(timeLimit, 10, 60);
			_inputTimeLimit.SetTextWithoutNotify(timeLimit.ToString());
		}

		public void ChangeRoundValue(int value)
		{
			int rounds = int.Parse(_inputRound.text) + value;
			rounds = HandleNumberInput(rounds, 1, 10);
			_inputRound.SetTextWithoutNotify(rounds.ToString());
		}

		public void ChangeMaxJokerValue(int value)
		{
			int maxJokers = int.Parse(_inputMaxJokers.text) + value;
			maxJokers = HandleNumberInput(maxJokers, 0, 6);
			_inputMaxJokers.SetTextWithoutNotify(maxJokers.ToString());
		}

		public void ChangeMoneyMultValue(int value)
		{
			int moneyMult = int.Parse(_inputMoneyMult.text) + value;
			moneyMult = HandleNumberInput(moneyMult, 1, 4);
			_inputMoneyMult.SetTextWithoutNotify(moneyMult.ToString());
		}

		public void ChangeJokerChanceValue(int value)
		{
			int jokerChance = int.Parse(_inputJokerChance.text) + value;
			jokerChance = HandleNumberInput(jokerChance, 0, 100);
			_inputJokerChance.SetTextWithoutNotify(jokerChance.ToString());
		}

#region Callbacks

		private void RoomNameChanged(string value)
		{
			HandleRoomName(value);

			if (_customRoomInfo.RoomName != value)
				_inputRoomName.SetTextWithoutNotify(_customRoomInfo.RoomName);
		}

		private void NumberInputChanged(string value, int origValue, TMP_InputField inputField, int min, int max)
		{
			if (!int.TryParse(value, out int newValue))
			{
				newValue = origValue;
			}
			else
			{
				newValue = HandleNumberInput(newValue, min, max);
			}

			inputField.SetTextWithoutNotify(newValue.ToString());
		}

		private void GameModeChanged(int value)
		{
			HandleGameModeValue(value);

			if ((int)_customRoomInfo.GameMode != value)
				_dropdownGameMode.SetValueWithoutNotify((int)_customRoomInfo.GameMode);
		}

		private void MapChanged(int value)
		{
			HandleMapValue(value);
			SetMapInfo();

			// if (_customRoomInfo.SceneIndex != value)
			// 	_dropdownScene.SetValueWithoutNotify(_customRoomInfo.SceneIndex);
		}

		private void CreatedRoom(Multiplayer multiplayer, bool success, Room room, string inviteCode)
		{	

			// gameObject.SetActive(false);
			// Lobby.SetActive(true);
			//StartCoroutine(Utility.UIElementSwitchEnum(new GameObject[]{gameObject}, new GameObject[]{Lobby}, touchBlock));
			_textInviteCode.transform.gameObject.SetActive(_toggleHideRoom.isOn ? true : false);
			_textInviteCode.text = _toggleHideRoom.isOn ? inviteCode : "";
		}

#endregion

		private void SetMapInfo()
		{
			MapInfo info = MapDescriptions.Instance.GetMapDescription(_customRoomInfo.SceneIndex);
			_imageMap.sprite = info.Image;
			_textMapInfo.text = info.Description;
			_textMapTitle.text = info.Title;

			if (_imageMap.sprite == null)
				_imageMap.sprite = MapDescriptions.Instance.DefaultImage;
		}

		private void HandleRoomName(string value)
		{
			_customRoomInfo.RoomName = value.Length > MaxNameLength ? value.Substring(0, MaxNameLength) : value;
		}

		private int HandleNumberInput(int value, int min, int max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;

			return value;
		}

		private void HandleGameModeValue(int value)
		{
			_customRoomInfo.GameMode = Enum.IsDefined(typeof(GameMode), (GameMode)value) ? (GameMode)value : 0;
		}

		private void HandleMapValue(int value)
		{
			_customRoomInfo.SceneIndex = value;
		}

		public void Submit()
		{
			if (Multiplayer.InRoom)
			{
				StatusPopup.Instance.TriggerStatus("Invalid action!\nAlready in a room!");
				return;
			}

			// cardHolder.SetActive(true);
			// gameUI.SetActive(true);
			// GameObject Player = Instantiate(PlayerPrefab, GameObject.Find("Game Canvas").transform);
			// Player.name = Multiplayer.Me.Name;
			// Player.transform.SetSiblingIndex(0);



			bool maxPlayersValid = int.TryParse(_inputMaxPlayers.text, out int maxPlayers);
			ushort password = FormatPassword(_inputPassword.text);

			if (_toggleHideRoom.isOn)
				password = (ushort)UnityEngine.Random.Range(1, 1024);

			HandleRoomName(_inputRoomName.text);
			HandleGameModeValue(_dropdownGameMode.value);
			//HandleMapValue(_dropdownScene.value);


			_customRoomInfo.RoomName = _customRoomInfo.RoomName.Trim(); // Remove whitespaces

			if (_customRoomInfo.RoomName.Length > MaxNameLength)
			{
				StatusPopup.Instance.TriggerStatus($"Invalid value!\n[Room Name] can be no longer than '{MaxNameLength}' characters!");
				return;
			}

			if (!maxPlayersValid || maxPlayers > MaxPlayers || maxPlayers < 1)
			{
				StatusPopup.Instance.TriggerStatus($"Invalid value!\n[Max Players] need to be between 1 and {MaxPlayers}!");
				return;
			}

			Gameplayinfo.GetComponent<GameplayInfo>().maxHands = Int32.Parse(_inputStartingHands.text);
			Gameplayinfo.GetComponent<GameplayInfo>().maxDiscards = Int32.Parse(_inputStartingDiscards.text);
			Gameplayinfo.GetComponent<GameplayInfo>().gameMaxPlayers = Int32.Parse(_inputMaxPlayers.text);
			Gameplayinfo.GetComponent<GameplayInfo>().TimeLimit = Int32.Parse(_inputTimeLimit.text);
			Gameplayinfo.GetComponent<GameplayInfo>().gameRounds = Int32.Parse(_inputRound.text);
			Gameplayinfo.GetComponent<GameplayInfo>().gameMaxJokers = Int32.Parse(_inputMaxJokers.text);
			Gameplayinfo.GetComponent<GameplayInfo>().gameMoneyMult = Int32.Parse(_inputMoneyMult.text);
			Gameplayinfo.GetComponent<GameplayInfo>().gameJokerChance = Int32.Parse(_inputJokerChance.text);

			// Serialize custom info into a string and pass it as the name of the room.
			string roomInfo = Writer.SerializeAndPackString(_customRoomInfo);

			if (!_toggleHideRoom.isOn)
				Multiplayer.CreateRoom(roomInfo, _toggleHideRoom.isOn, password, true, true, (ushort)maxPlayers);
			else
				Multiplayer.CreatePrivateRoom(_customRoomInfo.RoomName, (ushort)MaxPlayers, true, true);
		}

		public static ushort FormatPassword(string password)
		{
			if (password.Length == 0) return 0;

			if (int.TryParse(password, out int pin))
			{
				if (pin < 0) return (ushort)-pin;
				if (pin != 0 && (ushort)pin == 0) return (ushort)(pin >> 8);
				return (ushort)pin;
			}

			ushort hash = 0;
			foreach (char c in password)
			{
				hash += c;
			}

			if (hash == 0) return ushort.MaxValue;
			return hash;
		}

		/// <summary>
		/// Populates a dropdown with options based on an enum type.
		/// </summary>
		/// <param name="dropdown">The dropdown to populate,</param>
		/// <param name="enumType">The enum type to base the values on,</param>
		private void PopulateDropdownWithEnumValues(TMP_Dropdown dropdown, Type enumType)
		{
			if (enumType != null && enumType.IsEnum)
			{
				// Clear existing options
				dropdown.ClearOptions();

				// Retrieve enum names dynamically
				string[] names = Enum.GetNames(enumType);

				// Create a list of OptionData to store enum member names
				List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

				foreach (string name in names)
				{
					// Add the option to the dropdown
					options.Add(new TMP_Dropdown.OptionData(name));
				}

				dropdown.options = options;
			}
			else
			{
				Debug.LogError("Invalid enum type");
			}
		}

		private void PopulateDropdownWithSceneNames(TMP_Dropdown dropdown)
		{
			dropdown.ClearOptions();

			List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
			var descriptions = MapDescriptions.Instance.GetValidMapDescriptions();

			foreach (var item in descriptions)
			{
				options.Add(new TMP_Dropdown.OptionData(item.Title));
			}

			dropdown.options = options;
		}

#if UNITY_EDITOR
		private new void Reset()
		{
			base.Reset();

			if (EditorApplication.isPlaying)
				return;

			MapDescriptions.Instance.PopulateScenesIntoList();
		}
#endif
	}
}
