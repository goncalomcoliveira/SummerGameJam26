using UnityEngine;

namespace FlexibleUI {
    
    [CreateAssetMenu(
        fileName = "UIVisualTheme",
        menuName = "Flexible UI/Visual Theme")]
    public class UIVisualTheme : ScriptableObject {
        
        [Header("General")]
        public Color primaryColor = Color.white;
        public Color secondaryColor = Color.gray;
        public Color accentColor = Color.cyan;
        public Color backgroundColor = new(0.1f, 0.1f, 0.1f, 0.95f);

        [Header("Status Colors")]
        public Color successColor = new(0.2f, 0.8f, 0.4f);
        public Color infoColor = new(0.2f, 0.6f, 1f);
        public Color warningColor = new(1f, 0.7f, 0.1f);
        public Color errorColor = new(1f, 0.25f, 0.25f);

        [Header("Typography")]
        public Color titleColor = Color.white;
        public Color bodyColor = new(0.9f, 0.9f, 0.9f);

        public Sprite defaultIcon;
    }
}