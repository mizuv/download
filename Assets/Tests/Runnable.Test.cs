using System;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;
using Download.NodeSystem;
using UnityEngine;

public class RunnableTests {
    private class TestRunnable : Runnable {
        public TestRunnable(Folder? parent, string name, float runDuration) : base(parent, name) {
            _runDuration = runDuration;
        }

        private readonly float _runDuration;
        public override float RunDuration => _runDuration;

        public override string GetPrintString(string indent) {
            throw new NotImplementedException();
        }
    }

    [Test]
    public void RunProgress_ShouldBeZeroInitially() {
        // Arrange
        var runnable = new TestRunnable(null, "TestRunnable", 1000f);

        // Act & Assert
        Assert.AreEqual(0f, runnable.RunProgress.Value);
    }

    [Test]
    public void IsRunning_ShouldBeFalseInitially() {
        // Arrange
        var runnable = new TestRunnable(null, "TestRunnable", 1000f);

        // Act & Assert
        Assert.IsFalse(runnable.IsRunning.Value);
    }

    [UnityTest]
    public IEnumerator RunProgress_ShouldIncreaseOverTime() {
        // Arrange
        var runnable = new TestRunnable(null, "TestRunnable", 1000f);

        // Act
        runnable.StartRun();

        // Wait for a little longer than RUN_UPDATE_SECOND to ensure progress has been updated
        yield return new WaitForSeconds(0.2f);

        // Assert
        Assert.Greater(runnable.RunProgress.Value, 0f);

        // Cleanup
        runnable.StopRun();
    }

    [UnityTest]
    public IEnumerator RunProgress_ShouldResetAfterCompletion() {
        // Arrange
        var runnable = new TestRunnable(null, "TestRunnable", 500); // Set run duration to 0.5 seconds for quicker test

        // Act
        runnable.StartRun();

        // Wait for a little longer than the run duration to ensure completion
        yield return new WaitForSeconds(0.8f);
        // Assert
        Assert.AreEqual(0f, runnable.RunProgress.Value);
        Assert.IsFalse(runnable.IsRunning.Value);
    }

    [UnityTest]
    public IEnumerator IsRunning_ShouldBeTrueWhenStarted() {
        // Arrange
        var runnable = new TestRunnable(null, "TestRunnable", 1000f);

        // Act
        runnable.StartRun();

        // Wait for a frame to ensure the state is updated
        yield return null;

        // Assert
        Assert.IsTrue(runnable.IsRunning.Value);

        // Cleanup
        runnable.StopRun();
    }

    [UnityTest]
    public IEnumerator IsRunning_ShouldBeFalseWhenStopped() {
        // Arrange
        var runnable = new TestRunnable(null, "TestRunnable", 1000f);

        // Act
        runnable.StartRun();

        // Wait for a frame to ensure the state is updated
        yield return null;

        // Stop the runnable
        runnable.StopRun();

        // Assert
        Assert.IsFalse(runnable.IsRunning.Value);
    }
}
