# Проверка решений первого трека

Проверено 3 ноября 2026 года на Windows с .NET SDK 9.0.200 и целевой платформой .NET 8.

- 28 консольных проектов: сборка Release, 0 ошибок и 0 предупреждений.
- 28 из 28 демонстраций и встроенных проверок завершились успешно.
- 24 ноутбука: структура JSON и точное совпадение кода с Program.cs проверены.
- Комментарии в написанном C#-коде отсутствуют.
- Само ядро Polyglot Notebooks отдельно не запускалось; исполнен совпадающий исходный код консольных проектов.

Проверки включают отрицательные и недопустимые значения, полиморфные вызовы, операции с коллекциями, повторные добавления, подписку и отписку от событий, продажи сверх остатка, равенство компонентов и их перенос между компьютерами.

| Проект | Результат | SHA256 Program.cs |
| --- | --- | --- |
| Reviews/Review0 | PASS | `64af334b09dd82ca9a6fc05947d3c2d3c34ac539a260cae8770f306a662bec0f` |
| Reviews/Review1 | PASS | `a67b06750acfc32502d53130ae4f057c552924b878273f8ba00da82d46dcc270` |
| Reviews/Review2 | PASS | `d7ebdd3dec41aba2b7ebabe89b303922aa5ddf2a2aeba10d8dd8861ea24757be` |
| Reviews/Review3 | PASS | `84746b0bc160f4b358b3369892bd4424d4cb7b14b5349f5086f811cecf8caf34` |
| Sprint0/Task0 | PASS | `814e0918dde6394adb69cd55aad218485fde8c0a6e2a256ba220e37841dda4d3` |
| Sprint0/Task1 | PASS | `a2a8a8a4a354edb1bee61015cfeb726ef8ea72ef1530d34ac509812adcd0a3bb` |
| Sprint0/Task2 | PASS | `6406a8c380d4ba2947ddb1c6e6410d92f39f64baaa6110db5cef204bc426e6a0` |
| Sprint0/Task3 | PASS | `83c4ff329e90e2993ee26b8f5674905b6c9723b88eaab87650e4e49887b2caee` |
| Sprint1/Task0 | PASS | `412b6bd4c7fdef58875f13ea68121a0ffe33cfac1b3ec44fca7411380e8a71f6` |
| Sprint1/Task1 | PASS | `412b6bd4c7fdef58875f13ea68121a0ffe33cfac1b3ec44fca7411380e8a71f6` |
| Sprint1/Task2 | PASS | `412b6bd4c7fdef58875f13ea68121a0ffe33cfac1b3ec44fca7411380e8a71f6` |
| Sprint1/Task3 | PASS | `ad98935de8ad68764e94c7355fd486d463a50efe7dc2cf5a963b98eba355cb9f` |
| Sprint2/Task0 | PASS | `5f73dd0aceb80858fc670618275318e937898e8121b3f5e8dc5aa2cb5ac937bf` |
| Sprint2/Task1 | PASS | `9cf35d785965b5658f49536f35f938a0949ed5cd4b6bb88ab57a2d4c46f2703a` |
| Sprint2/Task2 | PASS | `2b9eddac6792ee2d5915c6f545e9ff00e895b735863714a139eb6ea8b91ce538` |
| Sprint2/Task3 | PASS | `fe26d9c61e1b04220b5f67d4981d655454e638adbf8447562f06685eb28c2896` |
| Sprint3/Task0 | PASS | `0909ac5be49ad54a76c197843bbe2ed2be43c058ebe1a21ef90694ac9c395dcd` |
| Sprint3/Task1 | PASS | `b833ecfc7000cf92e10d929f63cbe36141841225ff5931a1d579e6102000c338` |
| Sprint3/Task2 | PASS | `0c2108f95a19e342d3aa115ea9940a00e08c82c7fec0803eb9e71e8a758dd0c4` |
| Sprint3/Task3 | PASS | `3b8482066854ad9b424b79682aa7443b68720ba22a37f5cd1d115efb539651c1` |
| Sprint4/Task0 | PASS | `ae18c431a065e24eaeae5c48bc96589ba2f74242bc7d1229ee33ca221f66dbe1` |
| Sprint4/Task1 | PASS | `a4d00b0554df88735a54ef5df17b5403d6fa4af8d3711bc447ffe426aa317fb7` |
| Sprint4/Task2 | PASS | `3821d2b3c01aea8713904570a7c9d4ecc44167b78ba70aabe3247925115d4158` |
| Sprint4/Task3 | PASS | `20df28a537de8d761cf41944f5a0f51036cdfbe0017ca6be4a83d250bc2c8745` |
| Sprint5/Task0 | PASS | `5745048a7731552d5b820c21d4915b24426217733d4a499d511f5c3f441a73d6` |
| Sprint5/Task1 | PASS | `4b5cacba8b47032ede85b5569407220df855dc21e8bd7b1dc647aada635964ea` |
| Sprint5/Task2 | PASS | `f0d5ad77a5ef4d74a3284b22d14ae91e8ab60802bc2188775397f7002071f742` |
| Sprint5/Task3 | PASS | `0df46bf36a0e5bceed667c4f48164d87b83a130d33bc826b947bf359a58b616e` |
