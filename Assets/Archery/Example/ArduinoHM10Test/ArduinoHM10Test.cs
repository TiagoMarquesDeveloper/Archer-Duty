/* This is an example to show how to connect to 2 HM-10 devices
 * that are connected together via their serial pins and send data
 * back and forth between them.
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;

public class ArduinoHM10Test : MonoBehaviour
{
	public string DeviceName = "SuperAR_houyi5B2303";
	public string ServiceUUID = "00004990-9d11-4352-8984-15dfac4af403";
	public string Characteristic = "00004992-9d11-4352-8984-15dfac4af403";

	public Text HM10_Status;
	public Text BluetoothStatus;
	public GameObject PanelMiddle;
	public Text TextToSend;

	private int loaded = 0;

    public static int InputBow = 0;

	enum States
	{
		None,
		Scan,
		Connect,
		Subscribe,
		Unsubscribe,
		Disconnect,
		Communication,
	}

	private bool _workingFoundDevice = true;
	private bool _connected = false;
	private float _timeout = 0f;
	private States _state = States.None;
	private bool _foundID = false;

	// this is our hm10 device
	private string _hm10;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void OnButton(Button button)
	{
		if (button.name.Contains ("Send"))
		{
			if (string.IsNullOrEmpty (TextToSend.text))
			{
				BluetoothStatus.text = "Enter text to send...";
			}
			else
			{
				SendString (TextToSend.text);
			}
		}
		else if (button.name.Contains("Toggle"))
		{
			SendByte (0x01);
		}
	}

	void Reset ()
	{
		_workingFoundDevice = false;	// used to guard against trying to connect to a second device while still connecting to the first
		_connected = false;
		_timeout = 0f;
		_state = States.None;
		_foundID = false;
		_hm10 = null;
		PanelMiddle.SetActive (false);
	}

	void SetState (States newState, float timeout)
	{
		_state = newState;
		_timeout = timeout;
	}

	void StartProcess ()
	{
		BluetoothStatus.text = "Initializing...";

		Reset ();
		BluetoothLEHardwareInterface.Initialize (true, false, () => {
			
			SetState (States.Scan, 0.1f);
			BluetoothStatus.text = "Initialized";

		}, (error) => {
			
			BluetoothLEHardwareInterface.Log ("Error: " + error);
		});
	}

	// Use this for initialization
	void Start ()
	{
		HM10_Status.text = "";

		StartProcess ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (loaded == 1)
		{
			BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_hm10, ServiceUUID, Characteristic, null, (address, characteristicUUID, bytes) => {

				//HM10_Status.text = "Received Serial: " + ByteArrayToString(bytes);

				if (ByteArrayToString(bytes) == "d2fb1c7d1400000002")
				{
					InputBow = 1;//usar o arco
				}
				else if (ByteArrayToString(bytes) == "9fb7b07e1400000001")
				{
					InputBow = 2;//largar o arco
				}
				else if (ByteArrayToString(bytes) == "a6eafa98140000000e")
				{
					InputBow = 3;//click A
				}
				else if (ByteArrayToString(bytes) == "e2f4ed1c140000000d")
				{
					InputBow = 4;//leave A
				}
				else if (ByteArrayToString(bytes) == "2292d3ee1400000010")
				{
					InputBow = 5;//click B
				}
				else if (ByteArrayToString(bytes) == "430e28ae140000000f")
				{
					InputBow = 6;//leave B
				}

			});
		}

		if (_timeout > 0f)
		{
			_timeout -= Time.deltaTime;
			if (_timeout <= 0f)
			{
				_timeout = 0f;

				switch (_state)
				{
				case States.None:
					break;

				case States.Scan:
						
						BluetoothStatus.text = "Scanning for Bow...";

					BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {

						// we only want to look at devices that have the name we are looking for
						// this is the best way to filter out devices
						if (name.Contains (DeviceName))
						{
							_workingFoundDevice = true;

							// it is always a good idea to stop scanning while you connect to a device
							// and get things set up
							BluetoothLEHardwareInterface.StopScan ();
							BluetoothStatus.text = "";

							// add it to the list and set to connect to it
							_hm10 = address;

							HM10_Status.text = "Found Bow";

							SetState (States.Connect, 0.5f);

							_workingFoundDevice = false;
						}

					}, null, false, false);
					break;

				case States.Connect:
					// set these flags
					_foundID = false;

					HM10_Status.text = "Connecting to Bow";

					// note that the first parameter is the address, not the name. I have not fixed this because
					// of backwards compatiblity.
					// also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
					// the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
					// large enough that it will be finished enumerating before you try to subscribe or do any other operations.
					BluetoothLEHardwareInterface.ConnectToPeripheral (_hm10, null, null, (address, serviceUUID, characteristicUUID) => {

						if (IsEqual (serviceUUID, ServiceUUID))
						{
							// if we have found the characteristic that we are waiting for
							// set the state. make sure there is enough timeout that if the
							// device is still enumerating other characteristics it finishes
							// before we try to subscribe
							if (IsEqual (characteristicUUID, Characteristic))
							{
								_connected = true;
								SetState (States.Subscribe, 2f);

								HM10_Status.text = "Connected to Bow";
								loaded = 1;
                                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                            }
						}
					}, (disconnectedAddress) => {
						BluetoothLEHardwareInterface.Log ("Device disconnected: " + disconnectedAddress);
						HM10_Status.text = "Disconnected";
					});
					break;

				case States.Subscribe:
					HM10_Status.text = "Subscribing to Bow";

					BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (_hm10, ServiceUUID, Characteristic, null, (address, characteristicUUID, bytes) => {

						//HM10_Status.text = "Received Serial: " + ByteArrayToString(bytes);

                        if (ByteArrayToString(bytes) == "d2fb1c7d1400000002")
                        {
                            InputBow = 1;//usar o arco
                        }else if (ByteArrayToString(bytes) == "9fb7b07e1400000001")
                        {
                            InputBow = 2;//largar o arco
                        }else if (ByteArrayToString(bytes) == "a6eafa98140000000e")
                        {
                            InputBow = 3;//click A
                        }
                        else if (ByteArrayToString(bytes) == "e2f4ed1c140000000d")
                        {
                            InputBow = 4;//leave A
                        }
                        else if (ByteArrayToString(bytes) == "2292d3ee1400000010")
                        {
                            InputBow = 5;//click B
                        }
                        else if (ByteArrayToString(bytes) == "430e28ae140000000f")
                        {
                            InputBow = 6;//leave B
                        }

					});

					// set to the none state and the user can start sending and receiving data
					_state = States.None;
					HM10_Status.text = "Waiting...";

					PanelMiddle.SetActive (true);
					break;

				case States.Unsubscribe:
					BluetoothLEHardwareInterface.UnSubscribeCharacteristic (_hm10, ServiceUUID, Characteristic, null);
					SetState (States.Disconnect, 4f);
					break;

				case States.Disconnect:
					if (_connected)
					{
						BluetoothLEHardwareInterface.DisconnectPeripheral (_hm10, (address) => {
							BluetoothLEHardwareInterface.DeInitialize (() => {
								
								_connected = false;
								_state = States.None;
							});
						});
					}
					else
					{
						BluetoothLEHardwareInterface.DeInitialize (() => {
							
							_state = States.None;
						});
					}
					break;
				}
			}
		}
	}

	string FullUUID (string uuid)
	{
		return "0000" + uuid + "-0000-1000-8000-00805F9B34FB";
	}
	
	bool IsEqual(string uuid1, string uuid2)
	{
		if (uuid1.Length == 4)
			uuid1 = FullUUID (uuid1);
		if (uuid2.Length == 4)
			uuid2 = FullUUID (uuid2);

		return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
	}

    public static string ByteArrayToString(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }

    void SendString(string value)
	{
		var data = Encoding.UTF8.GetBytes (value);
		// notice that the 6th parameter is false. this is because the HM10 doesn't support withResponse writing to its characteristic.
		// some devices do support this setting and it is prefered when they do so that you can know for sure the data was received by 
		// the device
		BluetoothLEHardwareInterface.WriteCharacteristic (_hm10, ServiceUUID, Characteristic, data, data.Length, false, (characteristicUUID) => {

			BluetoothLEHardwareInterface.Log ("Write Succeeded");
		});
	}

	void SendByte (byte value)
	{
		byte[] data = new byte[] { value };
		// notice that the 6th parameter is false. this is because the HM10 doesn't support withResponse writing to its characteristic.
		// some devices do support this setting and it is prefered when they do so that you can know for sure the data was received by 
		// the device
		BluetoothLEHardwareInterface.WriteCharacteristic (_hm10, ServiceUUID, Characteristic, data, data.Length, false, (characteristicUUID) => {
			
			BluetoothLEHardwareInterface.Log ("Write Succeeded");
		});
	}
}
