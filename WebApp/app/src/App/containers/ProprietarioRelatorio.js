import React from 'react';
import axios from 'axios';
import Maps from './Maps';
import './Proprietarios.css'
import { Switch, Route, Link } from 'react-router-dom';

class ProprietarioRelatorio extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            relatorios: ''
        };

       this.printRelatorios = this.printRelatorios.bind(this);
    }

    componentDidMount()
    {
        this.loadRelatorio();
    }

    async loadRelatorio()
    {
        await axios.get('https://localhost:44301/proprietarios/Terrenos/Inspecoes', {
            params: {
                Username: this.props.user.username,
                Idterreno: this.props.selected.id_Terreno
            },
            headers: {
                "Authorization": this.state.auth
            }
        })
        .then(response => {
            this.setState({relatorios: response.data});
            //console.log(response.data);
        }) 
        .catch(response => {
            console.log(response);
        })
    }

    printRelatorios()
    {
        return (this.state.relatorios.length > 0 ? this.state.relatorios.map((relat, index) =>
        <div style={{textAlign: "Left"}} class="list-group-item flex-column align-items-start">
            <div class="d-flex w-100 justify-content-between">
                <h6 class="mb-1">{relat.resultado === 1? 'Aprovado' : 'Não Aprovado'}</h6>
            </div>
             <p class="mb-1">{relat.relatorio}</p>
             <small class="text-muted">{relat.dataHora.substring(11,16) + " - " + relat.dataHora.substring(8,10)
             + "/" + relat.dataHora.substring(5,7) + "/" + relat.dataHora.substring(0,4)}</small>
        </div>
    ) : <div><p className="notif-page-text">Ainda não foram realizadas inspeções ao seu terreno.</p></div>)
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
                            <div style={{marginTop: '4%'}} >
                                <h5 className="card-text login-text">Relatórios</h5>
                                <p>{this.props.selected.morada.replace(/\|/g,",") + '-' + this.props.selected.cod_postal}</p>
                                <div class="list-group">
                                    {this.printRelatorios()}
                                </div>
                                <div style={{marginTop: "5%", textAlign:"Left"}}>
                                    <Link to='/' class="btn login-btn btn-success btn-sm">
                                        Voltar
                                    </Link>
                                </div>
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

export default ProprietarioRelatorio;