//using UnityEngine;
//using System.Linq;
//using UnityEngine.SceneManagement;

//public class CreateRoomMenu : MonoBehaviour
//{
//    private InputTextField nameField;

//    private void Awake()
//    {
//        nameField = FindObjectsOfType<InputTextField>().Single(x => x.name == "NameInputField");
//    }

//    public void StartRoom()
//    {
//        ValidateInputFields();

//        if (IsFieldsValid)
//        {
//            SkirmishRoom.IAmServer = true;
//            SkirmishRoom.RoomName = nameField.Text;
//            SceneManager.LoadScene("Test", LoadSceneMode.Single);
//        }
//    }

//    public void BackToRooms()
//    {
//        SceneManager.LoadScene("RoomsMenu", LoadSceneMode.Single);
//    }

//    private void ValidateInputFields()
//    {
//        nameField.IsValid = !string.IsNullOrEmpty(nameField.Text);
//    }

//    private bool IsFieldsValid
//    {
//        get
//        {
//            return nameField.IsValid;
//        }
//    }
//}
