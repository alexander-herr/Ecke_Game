using System.Collections;
using UnityEngine;

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

    // Start is called before the first frame update
    private void Start()
    {
        _screenSize = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.position = new Vector3(_rigidbody.position.x, _rigidbody.position.y, 0);
        _initialPosition = _rigidbody.position;

        _audioSource = gameObject.AddComponent<AudioSource>();
        _obstacleClip = (AudioClip) Resources.Load("FX/ObstacleCollision");
        _targetClip = (AudioClip) Resources.Load("FX/TargetCollision");
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
        if (collision.transform.gameObject.name.Contains("Obstacle")) _audioSource.PlayOneShot(_obstacleClip);
        if (collision.transform.gameObject.name.Contains("Line")) _audioSource.PlayOneShot(_obstacleClip);
        if (collision.transform.gameObject.name.Contains("Target")) _audioSource.PlayOneShot(_targetClip);

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
        }
    }

    /*public static bool CheckOffScreen()
    {
        return (_rigidbody.position.x < -_screenSize.x - 1 || _rigidbody.position.x > _screenSize.x + 1 ||
                _rigidbody.position.y < -_screenSize.y - 1 || _rigidbody.position.y > _screenSize.y + 1);
    }*/

    public static bool CheckOffScreen()
    {
        return (_rigidbody.position.x < -_screenSize.x - 1 || _rigidbody.position.x > _screenSize.x + 1 ||
                          _rigidbody.position.y < -_screenSize.y - 1 || _rigidbody.position.y > _screenSize.y + 1);
    }
}