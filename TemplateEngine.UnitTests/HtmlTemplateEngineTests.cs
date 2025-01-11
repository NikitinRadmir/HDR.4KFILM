using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using TemlateEngine;

[TestFixture]
public class HtmlTemplateEngineTests
{
    private HtmlTemplateEngine _engine;
    private string _testDirectory;

    [SetUp]
    public void Setup()
    {
        _engine = new HtmlTemplateEngine();
        _testDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, "HtmlTemplates");
        if (!Directory.Exists(_testDirectory))
        {
            Directory.CreateDirectory(_testDirectory);
        }
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }

    [Test]
    public void Render_StringData_ShouldReplacePlaceholder()
    {
        // Arrange
        var template = "<html><body><p>Hello, {{Name}}!</p></body></html>";
        var str = "{{Name}}";
        var data = "World";
        var expectedHtml = "<html><body><p>Hello, World!</p></body></html>";

        // Act
        var htmlFilePath = CreateHtmlFile("Render_StringData_ShouldReplacePlaceholder", template);
        var fileInfo = new FileInfo(htmlFilePath);
        var result = _engine.Render(fileInfo, new { Name = data });

        // Assert
        Assert.AreEqual(expectedHtml, result);
    }

    [Test]
    public void Render_Stream_ShouldRenderTemplateFromStream()
    {
        // Arrange
        var template = "<html><body><p>Hello, {{Name}}!</p></body></html>";
        var obj = new Group { Name = "TestGroup", Students = new List<Student> { new Student { Id = 1, Name = "John Doe", Gender = true } } };
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(template));
        var expectedHtml = "<html><body><p>Hello, TestGroup!</p></body></html>";

        // Act
        var result = _engine.Render(stream, obj);

        // Assert
        Assert.AreEqual(expectedHtml, result);
    }

    [Test]
    public void HandleLoops_ShouldRenderLoopContent()
    {
        // Arrange
        var template = "<html><body><ul>{%for% student %in% Students}<li>{{student.Name}}</li>{%/for%}</ul></body></html>";
        var obj = new Group { Name = "TestGroup", Students = new List<Student> { new Student { Id = 1, Name = "John Doe", Gender = true }, new Student { Id = 2, Name = "Jane Doe", Gender = false } } };
        var expectedHtml = "<html><body><ul><li>John Doe</li><li>Jane Doe</li></ul></body></html>";

        // Act
        var htmlFilePath = CreateHtmlFile("HandleLoops_ShouldRenderLoopContent", template);
        var fileInfo = new FileInfo(htmlFilePath);
        var result = _engine.Render(fileInfo, obj);

        // Assert
        Assert.AreEqual(expectedHtml, result);
    }

    private string CreateHtmlFile(string testName, string template)
    {
        var htmlFilePath = Path.Combine(_testDirectory, $"{testName}.html");
        File.WriteAllText(htmlFilePath, template);
        return htmlFilePath;
    }
}
