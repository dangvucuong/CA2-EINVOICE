import { Box, Flash } from '@primer/react';
import { useParams } from 'react-router-dom';
import { useLocation } from 'react-router-dom';
import { hoaDonApi } from '../../api/hoa-don/hoaDonApi';
import { NotifyHelper } from '../../helpers/toast';
import { useEffect, useState } from 'react';
import HoaDonView from '../hoa-don-form/HoaDonView';
function useQuery() {
    return new URLSearchParams(useLocation().search);
}
const HoaDonViewPage = () => {
    const { id }: any = useParams();
    let query = useQuery();
    let hash = query.get('hash');
    const [isNotValidLink, setIsNotValidLink] = useState(false);
    const [hoaDonId, setHoaDonId] = useState(0);


    useEffect(() => {
        validateAsync();
    }, [id, hash])
    const validateAsync = async () => {
        const res = await hoaDonApi.validateViewLink(id, hash ?? "");
        if (res.is_success) {
            setHoaDonId(id)
            setIsNotValidLink(false)

        } else {
            NotifyHelper.Error("Đường dẫn không hợp lệ")
            setIsNotValidLink(true)
        }
    }

    return (
        <Box sx={{
            p: 3,
            overflow: "auto",
            height: window.innerHeight
        }}>
            {isNotValidLink &&
                <Flash variant='danger'>
                    Đường dẫn không hợp lệ
                </Flash>
            }
            {!isNotValidLink && hoaDonId > 0 &&
                <HoaDonView id={hoaDonId} />
            }
        </Box>
    );
};

export default HoaDonViewPage;