using UnityEngine;

namespace Helios.GUI {
    public class ShowFxWhenClickedOnScreen : MonoBehaviour {
        private ParticleSystem[] particles;
        private Vector2 mousePos;
        private int indexParticle = 0;

        private void Start() {
            particles = gameObject.transform.GetComponentsInChildren<ParticleSystem>();
        }

        private void LateUpdate() {
            if (Input.GetMouseButtonDown(0)) {
                mousePos = Input.mousePosition;
                particles[indexParticle].transform.position = new Vector3(mousePos.x, mousePos.y, 0);
                particles[indexParticle].Play();

                indexParticle++;
                if (indexParticle >= particles.Length) {
                    indexParticle = 0;
                }
            }
        }
    }
}