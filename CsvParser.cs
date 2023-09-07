// CSV Parser made by Julian Vogel

using System.Collections.Generic;

namespace Tools;

public class CsvParser
{
    private readonly string[][] _output;

    private readonly char[] _chars;
    private string _current;
    private bool _quote;
    private int _currentIndex;

    public CsvParser(string input)
    {
        _current = "";
        _chars = input.ToCharArray();
        var list = new List<string[]>();
        while (_currentIndex < _chars.Length)
        {
            list.Add(ParseLine().ToArray());
        }
        _output = list.ToArray();
    }

    private List<string> ParseLine()
    {
        var list = new List<string>();
        while (_currentIndex < _chars.Length)
        {
            switch (_chars[_currentIndex])
            {
                case '"':
                    if (_chars[_currentIndex + 1] == '"')
                    {
                        _current += '"';
                        _currentIndex++;
                    } else
                        _quote = !_quote;
                    break;
                case ',':
                    if (_quote)
                    {
                        _current += ',';
                        break;
                    }
                    list.Add(_current);
                    _current = "";
                    break;
                case '\n':
                    if (!_quote)
                    {
                        list.Add(_current);
                        _current = "";
                        _currentIndex++;
                        return list;
                    }
                    _current += "\n";
                    break;
                default:
                    _current += _chars[_currentIndex];
                    break;
            }
            _currentIndex++;
        }
        if(_currentIndex == _chars.Length)
            list.Add(_current);
        return list;
    }

    public string[][] GetTable()
    {
        return _output;
    }

    public int GetHeight()
    {
        return _output.Length;
    }

    public int GetWidth()
    {
        return _output.Length == 0 ? 0 : _output[0].Length;
    }
}
