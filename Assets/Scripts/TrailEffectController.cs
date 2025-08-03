using UnityEngine;

public class TrailEffectController : MonoBehaviour
{
    private ParticleSystem ps;
    public ParticleController particleController;
    // Stage 1: Red to Orange
    Gradient gradientStage1;
    // Stage 2: Orange to Yellow
    Gradient gradientStage2;
    // Stage 3: Yellow to White
    Gradient gradientStage3;
    // Stage 4: White to Blue
    Gradient gradientStage4;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        gradientStage1 = new Gradient();
        gradientStage2 = new Gradient();
        gradientStage3 = new Gradient();
        gradientStage4 = new Gradient();

        Color colorOrange = new Color(1.0f, 0.6f, 0.0f);
        Color lightBlue = new Color(0.2f, 0.2f, 1.0f);

        // Blend color from red at 0% to orange at 100%
        var redOrangeColors = new GradientColorKey[2];
        redOrangeColors[0] = new GradientColorKey(Color.red, 0.0f);
        redOrangeColors[1] = new GradientColorKey(colorOrange, 1.0f);

        var orangeYellowColors = new GradientColorKey[2];
        orangeYellowColors[0] = new GradientColorKey(colorOrange, 0.0f);
        orangeYellowColors[1] = new GradientColorKey(Color.yellow, 1.0f);

        var yellowWhiteColors = new GradientColorKey[2];
        yellowWhiteColors[0] = new GradientColorKey(Color.yellow, 0.0f);
        yellowWhiteColors[1] = new GradientColorKey(Color.white, 1.0f);

        var whiteBlueColors = new GradientColorKey[2];
        whiteBlueColors[0] = new GradientColorKey(Color.white, 0.0f);
        whiteBlueColors[1] = new GradientColorKey(lightBlue, 1.0f);

        // Constant Alphas
        var constantAlphas = new GradientAlphaKey[2];
        constantAlphas[0] = new GradientAlphaKey(1.0f, 0.0f);
        constantAlphas[1] = new GradientAlphaKey(1.0f, 1.0f);

        gradientStage1.SetKeys(redOrangeColors, constantAlphas);
        gradientStage2.SetKeys(orangeYellowColors, constantAlphas);
        gradientStage3.SetKeys(yellowWhiteColors, constantAlphas);
        gradientStage4.SetKeys(whiteBlueColors, constantAlphas);
    }

    void Update()
    {
        Color flameColor = CalculateColorFromForwardVelocity(particleController.forwardVelocity);
        ps.startColor = flameColor;
    }

    Color CalculateColorFromForwardVelocity(float forwardVelocity)
    {
        if (forwardVelocity < 0.0f)
        {
            // Shouldn't really get here since game would be over already
            return Color.red;
        }
        //float logVelocity = Mathf.Log(forwardVelocity, 4);
        float logVelocity = forwardVelocity / 10;
        if (logVelocity >= 0f && logVelocity < 1f)
        {
            return gradientStage1.Evaluate(logVelocity);
        } else if (logVelocity >= 1f && logVelocity < 2f)
        {
            return gradientStage2.Evaluate(logVelocity - 1f);
        } else if (logVelocity >= 2f && logVelocity < 3f)
        {
            return gradientStage3.Evaluate(logVelocity - 2f);
        } else if (logVelocity >= 3f && logVelocity < 4f)
        {
            return gradientStage4.Evaluate(logVelocity - 3f);
        } else
        {
            return gradientStage4.Evaluate(1f);
        }
    }
}