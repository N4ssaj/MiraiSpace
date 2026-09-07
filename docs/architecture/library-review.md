# MiraiSpace: обзор предложенных библиотек

6 сентября 2026. Рекомендации основаны на текущих README, NuGet-метаданных и локальных версиях проекта. «Кандидат» означает предмет обсуждения, а не решение установить пакет. Контрольная конфигурация MiraiSpace: .NET 10, Avalonia 12.1.1, ReactiveUI 23.2.28, ReactiveUI.SourceGenerators 3.2.0, DynamicData 9.4.31.

## Генератор наблюдений: что сделано

Найден [ReactiveUI.Binding.SourceGenerators](https://github.com/reactiveui/ReactiveUI.Binding.SourceGenerators). Это другой проект, чем уже установленный ReactiveUI.SourceGenerators для свойств и команд.

В Presentation и UI добавлены `ReactiveUI.Binding` и `ReactiveUI.Binding.SourceGenerators` версии **3.4.0**, с центральным управлением версиями. Анализатор подключён с `PrivateAssets="all"`.

Причина выбора: метаданные 3.4.0 требуют Splat 19.4.1 и System.Reactive 6.1.0, которые уже используются. Версия 4.1.0 требует Splat 21.0.0 и ReactiveUI.Primitives 7.2.0; обновление всего семейства сейчас не согласовано. В фактическом runtime-пакете 3.4.0 отсутствует папка analyzers, поэтому генератор подключён отдельным пакетом, несмотря на обещание автоматического включения в README.

Desktop собран с `EmitCompilerGeneratedFiles=true`: **0 ошибок, 65 предупреждений** от NuGet и анализаторов. Приложение и тесты не запускались. В assets подтверждены прежние Splat 19.4.1 и System.Reactive 6.1.0.

Генератор присутствует в компиляции, но существующие вызовы старого `WhenAnyValue` ещё не перешли на новый движок: сгенерированы вспомогательные файлы, а dispatch для наблюдений отсутствует. В исходниках закреплённой версии анализатор выбирает методы собственного движка. Миграцию call sites, разрешение возможных пересечений extension methods и проверку поведения нужно выполнить отдельным согласованным шагом. Ускорение приложения сейчас не измерено и не заявляется.

Источники версии: [NuGet runtime 3.4.0](https://www.nuget.org/packages/ReactiveUI.Binding/3.4.0), [NuGet generator 3.4.0](https://www.nuget.org/packages/ReactiveUI.Binding.SourceGenerators/3.4.0), [исходник извлечения вызовов](https://github.com/reactiveui/ReactiveUI.Binding.SourceGenerators/blob/ed90a91cadf313cc3f6fd0b46d7250507353a997/src/ReactiveUI.Binding.SourceGenerators/Helpers/ObservationExtractor.cs).

## Основные кандидаты

| Проект | Что даёт | Предложение для MiraiSpace |
| --- | --- | --- |
| [MPowerKit.Navigation](https://github.com/MPowerKit/Navigation) | MAUI-навигация, жизненный цикл, окна и регионы | Использовать как источник сценариев и разделения обязанностей. MAUI-компоненты непосредственно не устанавливать в Avalonia |
| [MessagePipe](https://github.com/Cysharp/MessagePipe) | Типизированные сообщения | Выбранное направление. До подключения определить время жизни брокеров и подписок |
| [Serilog.Extensions.Hosting](https://github.com/serilog/serilog-extensions-hosting) | Интеграция структурированного логирования с Generic Host | Выбранный основной вариант за `ILogger<T>` |
| [Xaml.Behaviors](https://github.com/wieslawsoltes/Xaml.Behaviors) | Behaviors, triggers и actions | Выбирать необходимые адаптеры событий и проверять совместимость с Avalonia 12. Не переносить в Behavior прикладную логику |
| [Autofac](https://autofac.readthedocs.io/en/latest/lifetime/working-with-scopes.html) | Явное управление вложенными lifetime scopes | Выбирать по модели владения сессией и страницами, а не по факту наличия плагинов |
| [HanumanInstitute.MvvmDialogs](https://github.com/mysteryx93/HanumanInstitute.MvvmDialogs) | ViewModel-ориентированные диалоги; есть интеграции DialogHost и ContentDialog | Основной референс для следующего этапа обсуждения диалогов |
| [ConsoleAppFramework](https://github.com/Cysharp/ConsoleAppFramework) | Генерируемый CLI с параметрами, фильтрами и интеграцией Host | Кандидат для текстового адаптера поверх прикладных действий. Для команд из динамических плагинов нужна отдельная проверка |

## Производительность и удобство

| Проект | Где полезен | Решение для обсуждения |
| --- | --- | --- |
| [ZString](https://github.com/Cysharp/ZString) | Частое построение/форматирование больших строк, буферы | Добавлять при конкретном интенсивном сценарии, например экспорте. Для нескольких подписей меню пользы пока не обосновано. У буферных builders есть обязанность освобождения |
| [ZLinq](https://github.com/Cysharp/ZLinq) | Вычисления над последовательностями, Span и деревьями с уменьшением аллокаций | Хороший кандидат для поиска/индексации и обработки данных. DynamicData продолжает отвечать за реактивные изменения UI-коллекций. Глобальный DropInGenerator отдельно обсуждать |
| [ValueTaskSupplement](https://github.com/Cysharp/ValueTaskSupplement) | Дополнительные операции вроде WhenAll, WhenAny и Lazy для ValueTask | Нужен при появлении конкретной композиции нескольких ValueTask. Сам выбор ValueTask для Initialize не требует этого пакета |
| [Humanizer](https://github.com/Humanizr/Humanizer) | Человекочитаемое представление дат, длительностей, чисел и строк | Полезен для «Недавних», истории и подсказок. Выбрать нужную локализацию и не использовать отображаемую строку как идентификатор |
| [ReactiveUI.Validation](https://github.com/reactiveui/ReactiveUI.Validation) | Реактивная валидация форм | Добавлять в функции с формами, сохраняя лёгкие общие базовые классы |

## Функциональные UI-компоненты

| Проект | Предлагаемая роль | Что проверить перед установкой |
| --- | --- | --- |
| [LiveMarkdown.Avalonia](https://github.com/DearVa/LiveMarkdown.Avalonia) | Потоковый текст AI и Markdown-предпросмотр | Совместимость версии с Avalonia 12; обновление builder на UI-потоке; поведение больших документов |
| [Coachlight.Avalonia](https://github.com/c3n9/Coachlight.Avalonia) | Обучающие туры по рабочему пространству и новым функциям плагина | Подключать после стабилизации навигации и целевых элементов. Предпочтительны стабильные ID целей вместо ссылок на контролы из ViewModel |
| [Mapsui](https://github.com/Mapsui/Mapsui) | Отдельная карта/географический модуль | Сначала выбрать географический сценарий и источник данных; ядру меню карта не нужна |
| [LiveCharts2](https://github.com/Live-Charts/LiveCharts2) | Графики в аналитическом модуле | Совместимость UI-пакета и реальная нагрузка данных. Не добавлять графики ради заполнения стартового экрана |
| [Nova.Avalonia.UI](https://github.com/jsuarezruiz/Nova.Avalonia.UI) | Дополнительные контролы: например, avatar и badge | README прямо указывает Avalonia 12.1.1+ в ветке 12, что соответствует версии проекта. Стилевое сочетание с DeltaDesign всё равно требует проверки |
| [FluentAvalonia](https://github.com/amwx/FluentAvalonia) | WinUI-подобные контролы, включая ContentDialog | README указывает требования для ветки Avalonia 11. Это не доказывает несовместимость с 12, но совместимость с нашей конфигурацией пока не подтверждена |

Из [awesome-avalonia](https://github.com/AvaloniaCommunity/awesome-avalonia) просмотрены категории тем, MVVM, компонентов, графиков и docking. Каталог полезен для поиска, но не подтверждает совместимость пакетов между собой. Для темы можно дальше сравнить SukiUI, Semi.Avalonia/Ursa и текущую базу — это дополнительное исследование, а не уже выбранная замена.

Моя рекомендация по собственной теме: сначала свои ресурсы и шаблоны в существующем `MiraiSpace.UI.Themes`, затем осознанно решать, нужен ли полный набор собственных ControlTheme. Такая последовательность позволит проверить характер интерфейса на меню, поиске, форме и Eremex DataGrid до большой работы над всеми контролами.

## Граница проделанной работы

Согласованные правила записаны в репозиторном AGENTS.md, документации архитектуры и локальных навыках. Frontmatter навыков сохранён. Стандартный Python-валидатор навыка не запустился из-за отсутствующего PyYAML; ссылки и изменённые правила проверены чтением. Прикладные `.cs` и `.axaml` в этом этапе обсуждения не менялись; из файлов сборки изменены только три файла подключения генератора.

Следующий разговор предлагаю вести по [записке об API](api-proposals-round-2.md): сначала сценарии меню, страницы и владение состоянием, затем окончательный выбор зависимостей.
