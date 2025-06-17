using System;
using System.Collections.Generic;
using UnityEngine;
public class StoryNode
{
    public List<string> TextSegments { get; }
    public List<Choice> Choices { get; }

    public StoryNode(IEnumerable<string> textSegments)
    {
        TextSegments = new List<string>(textSegments);
        Choices = new List<Choice>();
    }
}

public class Choice
{
    public string Description { get; }
    public StoryNode NextNode { get; }
    public System.Action<Player> Effect { get; }

    public Choice(string description, StoryNode nextNode, Action<Player> effect = null)
    {
        Description = description;
        NextNode = nextNode;
        Effect = effect;
    }
}


public class Story : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private StoryManager _storyManager;
    private StoryNode _currentNode;
    private int _currentTextIndex;

    void Start()
    {
        // 여러 문장으로 구성된 노드 예시
        var story1Start = new StoryNode(new[]
        {
            "2033년, 서울.",
            "당신은 폐허가 된 지하철역에 있습니다.",
            "주위를 둘러보니 어둠이 짙게 깔려 있습니다."
        });
        var story1A = new StoryNode(new[] { "당신은 어둠 속에서 무언가를 발견합니다." });
        var story1B = new StoryNode(new[] { "밖으로 나가기로 결정합니다." });

        story1Start.Choices.Add(new Choice(
            "주위를 살핀다",
            story1A,
            player => player.AddItem("손전등")
        ));
        story1Start.Choices.Add(new Choice(
            "밖으로 나간다",
            story1B,
            player => player.AddHealth(-10)
        ));

        var story2Start = new StoryNode(new[] { "2033년, 서울. 새로운 구역에 도착했습니다." });

        var stories = new List<StoryNode> { story1Start, story2Start };
        _storyManager = new StoryManager(stories);

        _currentNode = _storyManager.CurrentStory;
        _currentTextIndex = 0;
        ShowCurrentText();
    }

    void ShowCurrentText()
    {
        if (_currentTextIndex < _currentNode.TextSegments.Count)
        {
            Debug.Log(_currentNode.TextSegments[_currentTextIndex]);
        }
        else
        {
            ShowChoices();
        }
    }

    public void NextText()
    {
        _currentTextIndex++;
        ShowCurrentText();
    }

    void ShowChoices()
    {
        if (_currentNode.Choices.Count == 0)
        {
            Debug.Log("이 스토리는 끝났습니다.");
            if (_storyManager.MoveToNextStory())
            {
                _currentNode = _storyManager.CurrentStory;
                _currentTextIndex = 0;
                Debug.Log("다음 스토리가 시작됩니다.");
                ShowCurrentText();
            }
            else
            {
                Debug.Log("모든 스토리가 끝났습니다.");
            }
            return;
        }

        for (int i = 0; i < _currentNode.Choices.Count; i++)
        {
            Debug.Log($"{i + 1}. {_currentNode.Choices[i].Description}");
        }
    }

    public void Choose(int index)
    {
        if (index >= 0 && index < _currentNode.Choices.Count)
        {
            var choice = _currentNode.Choices[index];
            choice.Effect?.Invoke(_player);

            _currentNode = choice.NextNode;
            _currentTextIndex = 0;
            ShowCurrentText();
        }
    }
}

