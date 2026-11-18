const softwareList = [
    {
        id: 1,
        name: 'Интернет-магазин',
        version: '1.4.0',
        repository: 'https://gitlab.com/team/shop',
    },
    {
        id: 2,
        name: 'Мобильное приложение',
        version: '0.9.2',
        repository: 'https://gitlab.com/team/mobile',
    },
    {
        id: 3,
        name: 'Сайт кафедры',
        version: '2.1.0',
        repository: 'https://github.com/team/site',
    },
]

export default function SoftwarePage() {
    return (
        <>
            <h1>Программное обеспечение</h1>

            {softwareList.length === 0 ? (
                <div className="software-empty">
                    <p>ПО пока нет</p>
                    <p>Добавьте первое ПО, чтобы можно было создавать тикеты</p>
                </div>
            ) : (
                <div className="software-table-container">
                    <table className="software-table">
                        <thead>
                            <tr>
                                <th scope="col">Название</th>
                                <th scope="col">Версия</th>
                                <th scope="col">Репозиторий</th>
                            </tr>
                        </thead>
                        <tbody>
                            {softwareList.map((software) => (
                                <tr key={software.id}>
                                    <td>{software.name}</td>
                                    <td>{software.version}</td>
                                    <td>
                                        <a href={software.repository}>
                                            {software.repository.replace('https://', '')}
                                        </a>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </>
    )
}