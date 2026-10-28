#!/usr/bin/env bash
#
# Runs the Munshiganj_3D test suite without the Unity Editor.
#
# The engine independent code (Assets/Scripts/Core) and the EditMode tests are compiled
# with the Mono C# compiler and executed with the Mono runtime. The same test files are
# picked up by the Unity Test Runner inside the Editor (Window > General > Test Runner).
#
set -euo pipefail

cd "$(dirname "$0")/.."

BUILD_DIR="build/tests"
OUTPUT="$BUILD_DIR/MunshiganjTests.exe"

if ! command -v mcs >/dev/null 2>&1; then
    echo "error: the Mono C# compiler (mcs) is required to run the tests." >&2
    exit 127
fi

mkdir -p "$BUILD_DIR"

echo "Compiling the test assembly..."
mcs -optimize+ -out:"$OUTPUT" \
    Assets/Scripts/Core/Float3.cs \
    Assets/Scripts/Core/FollowMath.cs \
    Assets/Scripts/Core/PlaneMeshBuilder.cs \
    Assets/Scripts/Core/PlayerInputMath.cs \
    Assets/Tests/EditMode/CoreMathTests.cs \
    Assets/Tests/EditMode/PlaneMeshBuilderTests.cs \
    Tests/Portable/MiniNUnit.cs \
    Tests/Portable/PortableTestRunner.cs

echo "Running the tests..."
mono "$OUTPUT"
