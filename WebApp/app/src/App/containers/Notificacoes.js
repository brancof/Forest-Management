import React from 'react';
import axios from 'axios';
import './Notificacoes.css';

class Notificacoes extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            notifs: '',
        };

        this.printAllNotifs = this.printAllNotifs.bind(this);

    }

    componentDidMount() {
        this.loadNotifs();
    }

    async loadNotifs() {
        await axios.get('https://localhost:44301/' + this.props.accounttype + '/notificacoes', {
            params: {
                Username: this.props.user.username
            },
            headers: {
                "Authorization": this.state.auth
            }
        })
            .then(response => {
                this.setState({ notifs: response.data });
                console.log(response.data);
            })
            .catch(response => {
                console.log(response);
            })
    }


    printAllNotifs() {
        return (this.state.notifs.length > 0 ? this.state.notifs.map((notif, index) =>
            <div style={{ textAlign: "Left" }} className="list-group-item flex-column align-items-start notif-page-div" key={notif.id}>
                <div className="d-flex w-100 justify-content-between"><p className="notif-page-text">{notif.conteudo}</p></div>
                <p className="notif-page-date">{notif.dataEmissao.substring(11, 16) + " - " + notif.dataEmissao.substring(8, 10)
                    + "/" + notif.dataEmissao.substring(5, 7) + "/" + notif.dataEmissao.substring(0, 4)}</p>
            </div>
        ) : <div><p className="notif-page-text">Não tem notificações a apresentar.</p></div>)
    }

    render() {
        return (
            <div className="container login-container">
                <div className="row">
                    <div className="col"></div>
                    <div className="col-md-10">
                        <div className="card login-card">
                            <div className="card-block">
                                <h4 className="card-title login-title">{this.props.user.nome}</h4>
                                <h5 className="card-text login-text">Notificações</h5>
                                <div className="notifsdiv">
                                    {this.printAllNotifs()}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="col"></div>
                </div>
            </div>
        )
    }
}

export default Notificacoes;