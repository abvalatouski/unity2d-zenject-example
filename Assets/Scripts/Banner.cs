using System.Collections;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Banner
{
    [RequireComponent(typeof(Image))]
    public class Banner : MonoBehaviour
    {
        [Inject] private IProvider<Texture> textures;

        private Image image;
        private WaitForSeconds delayBetweenSlides;

        private void Awake()
        {
            image = GetComponent<Image>();
            delayBetweenSlides = new WaitForSeconds(1);
        }

        private void Start()
        {
            StartCoroutine(SlideShow());
        }

        private IEnumerator SlideShow()
        {
            while (true)
            {
                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    Task<Texture> task = textures.ProvideAsync(cancellationTokenSource.Token);
                    yield return new WaitUntil(() => task.IsCompleted);

                    SetTexture(task.Result);
                    yield return delayBetweenSlides;
                }
            }
        }

        private void SetTexture(Texture texture)
        {
            var material = new Material(image.material);
            material.mainTexture = texture;
            image.material = material;
        }
    }
}
