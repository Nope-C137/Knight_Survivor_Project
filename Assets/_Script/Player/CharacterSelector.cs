using UnityEngine;

namespace TD
{
    public class CharacterSelector : MonoBehaviour
    {
        public static CharacterSelector instance;

        public CharacterScriptableObject characterData;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static CharacterScriptableObject GetData()
        {
            return instance.characterData;
        }

        public void SelectedCharacter(CharacterScriptableObject character)
        {
            characterData = character;
        }

        public void DestroySingleton()
        {
            instance = null;
            Destroy(gameObject);
        }
    }
}