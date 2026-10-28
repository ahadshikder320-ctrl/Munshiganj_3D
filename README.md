# Munshiganj_3D
New PlaneMesh নির্বাচন করো।

Unity (C#) প্রজেক্ট — একটি ছোট third-person খেলার স্কেলেটন: রানটাইমে তৈরি নতুন PlaneMesh মেঝা,
ইনপুট-চালিত প্লেয়ার এবং একটি FollowCamera।

## প্রজেক্ট স্ট্রাকচার

```
Assets/
  Scenes/Munshiganj_3D.unity        # মূল সিন (GameBootstrap কম্পোনেন্ট)
  Scripts/
    Core/                            # ইঞ্জিন-স্বাধীন পিওর C# (ইউনিটি রেফারেন্স নেই)
      Float3.cs
      FollowMath.cs                  # Lerp / লার্প ফ্যাক্টর / ফলো পজিশন
      PlaneMeshBuilder.cs            # নতুন PlaneMesh (vertex, normal, UV, index)
      PlayerInputMath.cs             # yaw-সাপেক্ষিক মুভমেন্ট ডিরেকশন
    FollowCamera.cs                  # ব্যবহারকারীর কোড, কোর ম্যাথের উপর রিফ্যাক্টর করা
    GroundPlane.cs                   # রানটাইমে PlaneMesh তৈরি করে মেঝা সেট করে
    PlayerController.cs              # WASD/আই-জোয়াইন মুভমেন্ট
    GameBootstrap.cs                 # সিন বিল্ড: মেঝা + প্লেয়ার + ক্যামেরা + লাইট
  Tests/
    EditMode/                        # কোর ম্যাথ ও PlaneMesh ইউনিট টেস্ট
    PlayMode/                        # ক্যামেরা ও মেঝার রানটাইম টেস্ট
Packages/manifest.json
ProjectSettings/
Tests/Portable/                     # ইউনিটি ছাড়া (headless) টেস্ট রানার ও মিনি NUnit শিম
scripts/run-tests.sh                # CI/স্যান্ডবক্সে টেস্ট চালানোর স্ক্রিপ্ট
```

## টেস্ট চালানো

**ইউনিটি এডিটরে:** `Window > General > Test Runner` → EditMode / PlayMode ট্যাব → Run All।

**ইউনিটি ছাড়া (headless):**

```bash
bash scripts/run-tests.sh
```

স্ক্রিপ্টটি `Assets/Scripts/Core` ও `Assets/Tests/EditMode` ফাইলগুলো Mono C# কম্পাইলার (`mcs`) দিয়ে
কম্পাইল করে `Tests/Portable` থেকে চালায় এবং কোনো টেস্ট ব্যর্থ হলে non-zero exit code দেয়।

## নোট

- `FollowCamera`-এর লার্প ফ্যাক্টর 0..1 ক্ল্যাম্প করা হয় (Unity-র `Vector3.Lerp`-এর নিয়ম), ফলে
  দীর্ঘ ফ্রেমে ক্যামেরা ওভারশুট করে না।
- মেঝাটি ইউনিটির বিল্ট-ইন Plane প্রিফ্যাবের বদলে কোড দিয়ে তৈরি হয়, তাই সেগমেন্ট সংখ্যা,
  সাইজ ও রঙ ইনস্পেক্টর থেকে বদলানো যায়।
