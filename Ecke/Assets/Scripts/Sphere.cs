using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Sphere : MonoBehaviour
{
    private const float Velocity = 5f;
    private Vector3 _lastFrameVelocity;
    private static Rigidbody _rigidbody;
    public static bool StartMovement = true;
    private Vector3 _initialPosition;
    private static Vector2 _screenSize;
    private AudioSource _audioSource;
    private AudioClip _obstacleClip;
    private AudioClip _targetClip;

    private ArrayList pianoScale = new ArrayList();
    private int collisionCount = 0;

    // Start is called before the first frame update
    private void Start()
    {
        setAll();

        _screenSize = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.position = new Vector3(_rigidbody.position.x, _rigidbody.position.y, 0);
        _initialPosition = _rigidbody.position;

        _audioSource = gameObject.AddComponent<AudioSource>();
        _obstacleClip = (AudioClip) Resources.Load("FX/ObstacleCollision");
        _targetClip = (AudioClip) Resources.Load("FX/TargetCollision");

        AudioClip p0 = (AudioClip) Resources.Load("FX/Piano_Scale/piano-a_A_major");
        AudioClip p1 = (AudioClip) Resources.Load("FX/Piano_Scale/piano-b_B_major");
        AudioClip p2 = (AudioClip) Resources.Load("FX/Piano_Scale/piano-c_C_major");
        AudioClip p3 = (AudioClip) Resources.Load("FX/Piano_Scale/piano-d_D_major");
        AudioClip p4 = (AudioClip) Resources.Load("FX/Piano_Scale/piano-e_E_major");
        AudioClip p5 = (AudioClip) Resources.Load("FX/Piano_Scale/piano-f_F_major");
        AudioClip p6 = (AudioClip) Resources.Load("FX/Piano_Scale/piano-g_G_major");
        pianoScale.Add(p0);
        pianoScale.Add(p1);
        pianoScale.Add(p2);
        pianoScale.Add(p3);
        pianoScale.Add(p4);
        pianoScale.Add(p5);
        pianoScale.Add(p6);
    }

    // Update is called once per frame
    private void Update()
    {
        _lastFrameVelocity = _rigidbody.velocity;
        StartMoving();
        Restart();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.name.Contains("Obstacle")) _audioSource.PlayOneShot(_targetClip); // Variable umbenennen
        if (collision.transform.gameObject.name.Contains("Line")) _audioSource.PlayOneShot(_obstacleClip);
        //if (collision.transform.gameObject.name.Contains("Target")) _audioSource.PlayOneShot(_targetClip);

        if (collision.transform.gameObject.name.Contains("Target"))
        {
            _audioSource.PlayOneShot((AudioClip) pianoScale[collisionCount]);
            collisionCount++;
        }

        foreach (var contactPoint in collision.contacts)
        {
            _rigidbody.velocity = Vector3.Reflect(_lastFrameVelocity.normalized, contactPoint.normal) *
                                  Mathf.Max(Velocity, _lastFrameVelocity.magnitude);
            return;
        }
    }

    void StartMoving()
    {
        // Start moving the sphere if a line is drawn
        if (DrawLine.currLines == 1 && StartMovement)
        {
            _rigidbody.velocity = new Vector3(0, -10f, 0);
            StartMovement = false;
        }
    }

    void Restart()
    {
        if ((CheckOffScreen() && !Target.LevelDone) || DrawLine.RestartNewLine)
        {
            _rigidbody.position = _initialPosition;
            _rigidbody.velocity = Vector3.zero;
            StartMovement = true;
            var matSphere = GetComponent<Renderer>().material;
            var colorSphere = matSphere.color;
            colorSphere.a = 0;
            matSphere.color = colorSphere;
            StartCoroutine(Animations.FadeTo(matSphere, 1f, 0.3f));
            collisionCount = 0;
        }
    }

    /*public static bool CheckOffScreen()
    {
        return (_rigidbody.position.x < -_screenSize.x - 1 || _rigidbody.position.x > _screenSize.x + 1 ||
                _rigidbody.position.y < -_screenSize.y - 1 || _rigidbody.position.y > _screenSize.y + 1);
    }*/

    public bool CheckOffScreen()
    {
        return (transform.position.x < -_screenSize.x - 1 || transform.position.x > _screenSize.x + 1 ||
                transform.position.y < -_screenSize.y - 1 || transform.position.y > _screenSize.y + 1);
    }

    void setAll()
    {
        // Deactivate Point Light
        GameObject.Find("Point Light").SetActive(false);

        // Add Post Processing Bloom
        var mainCamera = GameObject.Find("Main Camera");
        mainCamera.AddComponent<PostProcessingBehaviour>();
        mainCamera.GetComponent<PostProcessingBehaviour>().profile = (PostProcessingProfile) Resources.Load("Materials/Post_Processing");
        PostProcessingProfile profile = mainCamera.GetComponent<PostProcessingBehaviour>().profile;
        profile.bloom.enabled = true;
    }
}