# Notatnik

Prosty notatnik w Avalonia. Zapisuje wpisy do pliku `notatki.md` w bieżącym
folderze uruchomienia aplikacji, w kodowaniu UTF-8 bez BOM.

## Uruchomienie

```powershell
dotnet run
```

Wpisz tytuł i treść, a następnie wybierz **Zapisz notatkę**. Wpisy są
dopisywane na początku pliku, więc najnowszy znajduje się na górze. Aplikacja
odczytuje zapisane wpisy przy uruchomieniu i pokazuje je na liście.

Format wpisu:

```text
## 2026-09-08 14:32 — Tytuł notatki
Treść notatki, może mieć kilka linii
```

Między wpisami znajduje się pusta linia. Obsługa zapisu wyświetla komunikat
z przyczyną, jeśli system nie pozwoli zapisać pliku.

## Ręczne sprawdzenie

1. Zapisz notatkę z polskimi znakami w tytule i treści; sprawdź `notatki.md`
   w edytorze tekstu.
2. Zapisz co najmniej pięć notatek i sprawdź, że wszystkie są w pliku,
   najnowsza na początku.
3. Uruchom aplikację ponownie i sprawdź, że wpisy nadal są widoczne.
4. Ustaw `notatki.md` jako tylko do odczytu i spróbuj zapisać wpis. Aplikacja
   powinna pozostać otwarta i pokazać powód błędu; po zdjęciu atrybutu zapis
   powinien działać.

## Dodatkowa wskazówka o promptach

Anthropic zaleca oznaczanie instrukcji, kontekstu i danych wejściowych tagami
XML, gdy prośba zawiera różne rodzaje treści. Przy prośbie o zmianę notatnika
pozwala to oddzielić wymagania dotyczące aplikacji od przykładowej zawartości
notatki; sama wskazówka nie zmienia formatu pliku.

Źródło: [Claude prompting best practices](https://platform.claude.com/docs/en/build-with-claude/prompt-engineering/claude-prompting-best-practices)
— odczytano 29.10.2026.
