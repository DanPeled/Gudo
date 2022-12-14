using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Types {
    public static Types instance = new Types();
    public List < string > names = new List < string > () {
        "Wizard",
        "Warrior",
        "Archer"
    };

    public List < int > dmg = new List < int > () {
        15,
        20,
        17
    };
    public  List<object> Get_(int index) {
        return new List<object>(){names[index] ,dmg[index]};
    }
}