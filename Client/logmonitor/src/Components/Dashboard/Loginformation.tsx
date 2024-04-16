import { useEffect, useState } from "react";
import useFetch from "../../Hooks/UseFetch";
import { APIServerURL } from "../../Data/LogsData";
import './Loginformation.css';
import { logModels } from "../../Models/Models";
import { formatDateTime } from "../../Utils/Utils";


const Loginformation: React.FC = () => {
    const fetch = useFetch();
    const[LogData,setLogData] = useState<Array<logModels>>([]);

    useEffect(() => {
        (async () => {
            await getData();
        })()
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    const getData = async () => {
        const res = await fetch.getAsync({ url: `${APIServerURL}Log/getLogs`, returnType: 'JSON' })
        setLogData(res)
    }

    return (
        <div className="logInfowrapper">
            <table>
                <thead>
                    <tr>
                        <td>
                            Application Name
                        </td>
                        <td>
                            Date
                        </td>
                        <td>
                            Method
                        </td>
                        <td>
                            Type
                        </td>
                        <td>
                            Route
                        </td>
                        <td>
                            Elapsed Time
                        </td>
                    </tr>
                </thead>
                <tbody className="tbodyCls">
                    {
                        LogData?.length > 0 && (
                            LogData.map((value: logModels, indx: number) => (
                                <tr key={indx} className="hoverTr">
                                    <td>
                                        {value.applicationName}
                                    </td>
                                    <td>
                                        {formatDateTime('dd-MMM-yyyy hh:mm',new Date(value.date))}
                                    </td>
                                    <td>
                                        {value.method}
                                    </td>
                                    <td>
                                        {value.logType}
                                    </td>
                                    <td>
                                        {value.route}
                                    </td>
                                    <td>
                                        {value.elapsedTime} ms
                                    </td>
                                </tr>
                            ))
                        ) || (
                            <tr>
                                <td colSpan={6}>
                                    <b>No Records found !</b>
                                </td>
                            </tr>
                        )
                    }
                </tbody>
            </table>
        </div>
    )
}

export default Loginformation;