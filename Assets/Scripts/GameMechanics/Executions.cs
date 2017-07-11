using System;
using System.Linq;
using System.Reflection;
using GameMechanics;
using Other;
using UnityEngine;

public class Executions : MonoBehaviour
{
    public static void RotateCurrent()
    {
        var r = CommandBehaviors.self.currentGameObject.AddComponent<RotateGameObject>();
        r.angle = Vector3.up * 90;
    }

    public static void RotateCurrent(string s)
    {
        var mag = float.Parse(s);
        var r = CommandBehaviors.self.currentGameObject.AddComponent<RotateGameObject>();
        r.angle = Vector3.up * mag;
    }

    public static void RotateCurrent(string axis, string angle)
    {
        var angleValue = float.Parse(angle);
        switch (axis)
        {
            case "x":
                CommandBehaviors.self.currentGameObject.transform.Rotate(Vector3.right, angleValue);
                break;
            case "y":
                CommandBehaviors.self.currentGameObject.transform.Rotate(Vector3.up, angleValue);
                break;
            case "z":
                CommandBehaviors.self.currentGameObject.transform.Rotate(Vector3.forward, angleValue);
                break;

            default:
                throw new Exception("No Axis Found");
        }
    }

    public static void RotateCurrent(string s1, string s2, string s3)
    {
        var x = float.Parse(s1);
        var y = float.Parse(s2);
        var z = float.Parse(s3);
        var r = CommandBehaviors.self.currentGameObject.AddComponent<RotateGameObject>();
        r.angle = new Vector3(x, y, z);
    }

    public static void DestroyCurrent()
    {
        Destroy(CommandBehaviors.self.currentGameObject);

        var obj = Resources.Load("Destroy Particles") as GameObject;
        if (!obj) return;

        obj = Instantiate(obj);

        obj.transform.position = CommandBehaviors.self.currentGameObject.transform.position;
    }

    public static void ApplyRandomColor()
    {
        if (CommandBehaviors.self.currentGameObject.transform.GetComponent<RandomColor>())
            throw new Exception();

        var c = CommandBehaviors.self.currentGameObject.AddComponent<RandomColor>();
        c.waitTime = .1f;
    }

    public static void ApplyRandomColor(string wait)
    {
        if (CommandBehaviors.self.currentGameObject.transform.GetComponent<RandomColor>())
            throw new Exception();

        var waitTime = float.Parse(wait);
        var c = CommandBehaviors.self.currentGameObject.AddComponent<RandomColor>();
        c.waitTime = waitTime;
    }

    public static void ApplyColor(string r, string g, string b)
    {
        var c = new Color(float.Parse(r), float.Parse(g), float.Parse(b));
        CommandBehaviors.self.currentGameObject.GetComponent<MeshRenderer>().material.color = c;
    }

    public static void MovePosition(string x, string y, string z)
    {
        var pos = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
        CommandBehaviors.self.currentGameObject.transform.position += pos;
    }

    public static void Spawn(string path)
    {
        Instantiate(Resources.Load(path));
    }

    public static void AddComponet(string comp)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var typelist = assembly.GetTypes().Where(t => string.Equals(t.Namespace, "Other",
            StringComparison.Ordinal)).ToList();

        foreach (var type in typelist)
        {
            if (type.ToString() != "Other." + comp) continue;

            if (CommandBehaviors.self.currentGameObject.GetComponent(type))
                Destroy(CommandBehaviors.self.currentGameObject.GetComponent(type));
            CommandBehaviors.self.currentGameObject.AddComponent(type);
        }
    }

    public static void RemoveComponet(string comName)
    {
        var componets = CommandBehaviors.self.currentGameObject.GetComponents<Component>();
        foreach (var c in componets)
        {
            if (comName != c.GetType().ToString()) continue;

            Destroy(c);
            break;
        }
    }

    public static void FloatGameObject(string mag)
    {
        if (CommandBehaviors.self.currentGameObject.transform.GetComponent<Floating>())
            Destroy(CommandBehaviors.self.currentGameObject.transform.GetComponent<Floating>());

        var magValue = float.Parse(mag);

        var floatComp = CommandBehaviors.self.currentGameObject.AddComponent<Floating>();
        floatComp.magnitude = magValue;
    }
}
